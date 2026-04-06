using System.Globalization;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Shared.Mediator;
using WeChooz.TechAssessment.Application.Courses.Queries.GetCourseById;
using WeChooz.TechAssessment.Application.Courses.Queries.GetCourses;
using WeChooz.TechAssessment.Application.PublicSessions.Queries.GetPublicSessionDetail;
using WeChooz.TechAssessment.Application.PublicSessions.Queries.GetPublicSessions;

namespace WeChooz.TechAssessment.Application.Caching;

internal sealed class GetCoursesCacheBehavior(IDistributedCache cache) : IPipelineBehavior<GetCoursesQuery, GetCoursesResponse>
{
    private static readonly TimeSpan Ttl = TimeSpan.FromMinutes(10);
    private const string CacheKey = "query:courses:list:v1";

    public Task<GetCoursesResponse> HandleAsync(GetCoursesQuery input, Func<Task<GetCoursesResponse>> next, CancellationToken cancellationToken = default) =>
        RedisQueryCache.GetOrCreateAsync(cache, CacheKey, Ttl, next, cancellationToken);
}

internal sealed class GetCourseByIdCacheBehavior(IDistributedCache cache) : IPipelineBehavior<GetCourseByIdQuery, GetCourseByIdResponse?>
{
    private static readonly TimeSpan Ttl = TimeSpan.FromMinutes(10);

    public Task<GetCourseByIdResponse?> HandleAsync(GetCourseByIdQuery input, Func<Task<GetCourseByIdResponse?>> next, CancellationToken cancellationToken = default)
    {
        var key = $"query:courses:by-id:{input.CourseId}:v1";
        return RedisQueryCache.GetOrCreateNullableAsync(cache, key, Ttl, next, cancellationToken);
    }
}

internal sealed class GetPublicSessionsCacheBehavior(IDistributedCache cache) : IPipelineBehavior<GetPublicSessionsQuery, GetPublicSessionsResponse>
{
    private static readonly TimeSpan Ttl = TimeSpan.FromMinutes(2);

    public Task<GetPublicSessionsResponse> HandleAsync(GetPublicSessionsQuery input, Func<Task<GetPublicSessionsResponse>> next, CancellationToken cancellationToken = default)
    {
        var key = $"query:public-sessions:list:v1:{input.Audience?.ToString() ?? "null"}:{input.DeliveryMode?.ToString() ?? "null"}:{FormatDate(input.StartAfter)}:{FormatDate(input.StartBefore)}:{FormatDate(input.StartFrom)}:{FormatDate(input.StartTo)}";
        return RedisQueryCache.GetOrCreateAsync(cache, key, Ttl, next, cancellationToken);
    }

    private static string FormatDate(DateTime? date) => date?.ToUniversalTime().ToString("O", CultureInfo.InvariantCulture) ?? "null";
}

internal sealed class GetPublicSessionDetailCacheBehavior(IDistributedCache cache) : IPipelineBehavior<GetPublicSessionDetailQuery, GetPublicSessionDetailResponse?>
{
    private static readonly TimeSpan Ttl = TimeSpan.FromMinutes(5);

    public Task<GetPublicSessionDetailResponse?> HandleAsync(GetPublicSessionDetailQuery input, Func<Task<GetPublicSessionDetailResponse?>> next, CancellationToken cancellationToken = default)
    {
        var key = $"query:public-sessions:detail:{input.SessionId}:v1";
        return RedisQueryCache.GetOrCreateNullableAsync(cache, key, Ttl, next, cancellationToken);
    }
}

internal static class RedisQueryCache
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public static async Task<TResponse> GetOrCreateAsync<TResponse>(
        IDistributedCache cache,
        string cacheKey,
        TimeSpan ttl,
        Func<Task<TResponse>> next,
        CancellationToken cancellationToken)
    {
        var payload = await cache.GetStringAsync(cacheKey, cancellationToken);
        if (payload is not null)
        {
            var cached = JsonSerializer.Deserialize<TResponse>(payload, JsonOptions);
            if (cached is not null)
            {
                return cached;
            }
        }

        var fresh = await next();
        var serialized = JsonSerializer.Serialize(fresh, JsonOptions);
        await cache.SetStringAsync(cacheKey, serialized, BuildOptions(ttl), cancellationToken);
        return fresh;
    }

    public static async Task<TResponse?> GetOrCreateNullableAsync<TResponse>(
        IDistributedCache cache,
        string cacheKey,
        TimeSpan ttl,
        Func<Task<TResponse?>> next,
        CancellationToken cancellationToken)
    {
        var payload = await cache.GetStringAsync(cacheKey, cancellationToken);
        if (payload is not null)
        {
            var envelope = JsonSerializer.Deserialize<NullableCacheEnvelope<TResponse>>(payload, JsonOptions);
            if (envelope is not null && envelope.HasValue)
            {
                return envelope.Value;
            }
        }

        var fresh = await next();
        var serialized = JsonSerializer.Serialize(new NullableCacheEnvelope<TResponse>(true, fresh), JsonOptions);
        await cache.SetStringAsync(cacheKey, serialized, BuildOptions(ttl), cancellationToken);
        return fresh;
    }

    private static DistributedCacheEntryOptions BuildOptions(TimeSpan ttl) =>
        new() { AbsoluteExpirationRelativeToNow = ttl };

    private sealed record NullableCacheEnvelope<T>(bool HasValue, T? Value);
}
