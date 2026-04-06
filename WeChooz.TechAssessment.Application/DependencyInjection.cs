using Microsoft.Extensions.DependencyInjection;
using Shared.Mediator;
using Shared.Validation;
using WeChooz.TechAssessment.Application.Caching;
using WeChooz.TechAssessment.Application.Courses.Queries.GetCourseById;
using WeChooz.TechAssessment.Application.Courses.Queries.GetCourses;
using WeChooz.TechAssessment.Application.PublicSessions.Queries.GetPublicSessionDetail;
using WeChooz.TechAssessment.Application.PublicSessions.Queries.GetPublicSessions;

namespace WeChooz.TechAssessment.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, string redisConnectionString)
    {
        services.AddMediator();
        services.AddValidation();
        services.AddStackExchangeRedisCache(options => options.Configuration = redisConnectionString);
        services.AddScoped<IPipelineBehavior<GetCoursesQuery, GetCoursesResponse>, GetCoursesCacheBehavior>();
        services.AddScoped<IPipelineBehavior<GetCourseByIdQuery, GetCourseByIdResponse?>, GetCourseByIdCacheBehavior>();
        services.AddScoped<IPipelineBehavior<GetPublicSessionsQuery, GetPublicSessionsResponse>, GetPublicSessionsCacheBehavior>();
        services.AddScoped<IPipelineBehavior<GetPublicSessionDetailQuery, GetPublicSessionDetailResponse?>, GetPublicSessionDetailCacheBehavior>();
        return services;
    }
}
