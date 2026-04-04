using Dapper;
using Microsoft.Data.SqlClient;
using WeChooz.TechAssessment.Application.Abstractions.Data;
using WeChooz.TechAssessment.Domain.ReadModels;
using WeChooz.TechAssessment.Domain.Repositories;
using WeChooz.TechAssessment.Domain.Sessions;
using WeChooz.TechAssessment.Infrastructure.Data.Sql;

namespace WeChooz.TechAssessment.Infrastructure.Data.Repositories;

public sealed class SessionRepository(IDbConnectionFactory connectionFactory) : ISessionRepository
{
    public async Task<IReadOnlyList<PublicSessionListItemResult>> ListPublicAsync(ListPublicSessionsFilter filter, CancellationToken cancellationToken = default)
    {
        await using var conn = await OpenAsync(cancellationToken);
        var rows = await conn.QueryAsync<PublicSessionListItemResult>(
            new CommandDefinition(
                SessionSql.ListPublic,
                new
                {
                    Audience = filter.Audience.HasValue ? (byte?)filter.Audience.Value : null,
                    DeliveryMode = filter.DeliveryMode.HasValue ? (byte?)filter.DeliveryMode.Value : null,
                    StartAfter = filter.StartAfter,
                    StartBefore = filter.StartBefore,
                    StartFrom = filter.StartFrom,
                    StartTo = filter.StartTo,
                },
                cancellationToken: cancellationToken));
        return rows.AsList();
    }

    public async Task<PublicSessionDetailResult?> GetPublicDetailAsync(int sessionId, CancellationToken cancellationToken = default)
    {
        await using var conn = await OpenAsync(cancellationToken);
        return await conn.QuerySingleOrDefaultAsync<PublicSessionDetailResult>(
            new CommandDefinition(SessionSql.GetPublicDetail, new { SessionId = sessionId }, cancellationToken: cancellationToken));
    }

    public async Task<IReadOnlyList<AdminSessionListItemResult>> ListAdminAsync(CancellationToken cancellationToken = default)
    {
        await using var conn = await OpenAsync(cancellationToken);
        var rows = await conn.QueryAsync<AdminSessionListItemResult>(new CommandDefinition(SessionSql.ListAdmin, cancellationToken: cancellationToken));
        return rows.AsList();
    }

    public async Task<AdminSessionDetailResult?> GetAdminByIdAsync(int sessionId, CancellationToken cancellationToken = default)
    {
        await using var conn = await OpenAsync(cancellationToken);
        return await conn.QuerySingleOrDefaultAsync<AdminSessionDetailResult>(
            new CommandDefinition(SessionSql.GetAdminById, new { SessionId = sessionId }, cancellationToken: cancellationToken));
    }

    public async Task<int> InsertAsync(int courseId, DateTime startDate, SessionDeliveryMode deliveryMode, CancellationToken cancellationToken = default)
    {
        await using var conn = await OpenAsync(cancellationToken);
        return await conn.QuerySingleAsync<int>(
            new CommandDefinition(
                SessionSql.Insert,
                new { CourseId = courseId, StartDate = startDate, DeliveryMode = (byte)deliveryMode },
                cancellationToken: cancellationToken));
    }

    public async Task<bool> UpdateAsync(int sessionId, int courseId, DateTime startDate, SessionDeliveryMode deliveryMode, CancellationToken cancellationToken = default)
    {
        await using var conn = await OpenAsync(cancellationToken);
        var affected = await conn.ExecuteAsync(
            new CommandDefinition(
                SessionSql.Update,
                new
                {
                    SessionId = sessionId,
                    CourseId = courseId,
                    StartDate = startDate,
                    DeliveryMode = (byte)deliveryMode,
                },
                cancellationToken: cancellationToken));
        return affected > 0;
    }

    public async Task DeleteAsync(int sessionId, CancellationToken cancellationToken = default)
    {
        await using var conn = await OpenAsync(cancellationToken);
        await conn.ExecuteAsync(new CommandDefinition(SessionSql.Delete, new { SessionId = sessionId }, cancellationToken: cancellationToken));
    }

    private async Task<SqlConnection> OpenAsync(CancellationToken cancellationToken)
    {
        var conn = (SqlConnection)connectionFactory.CreateConnection();
        await conn.OpenAsync(cancellationToken);
        return conn;
    }
}
