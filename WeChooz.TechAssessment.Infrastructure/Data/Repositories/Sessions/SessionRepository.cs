using Dapper;
using WeChooz.TechAssessment.Application.Interfaces.Sessions;
using WeChooz.TechAssessment.Domain.Sessions;
using WeChooz.TechAssessment.Infrastructure.Data.Repositories.Sessions.ReadModels;

namespace WeChooz.TechAssessment.Infrastructure.Data.Repositories.Sessions;

public sealed class SessionRepository(IDbConnectionFactory connectionFactory) : ISessionRepository
{
    public async Task<IReadOnlyList<PublicSessionListing>> ListPublicAsync(PublicSessionListCriteria criteria, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            SELECT
                s.SessionId,
                c.Name AS CourseName,
                c.ShortDescription,
                c.CseAudience,
                s.StartDate,
                c.DurationDays,
                s.DeliveryMode,
                CASE
                    WHEN c.MaxCapacity - ISNULL(pc.ParticipantCount, 0) < 0 THEN 0
                    ELSE c.MaxCapacity - ISNULL(pc.ParticipantCount, 0)
                END AS RemainingSeats,
                c.TrainerFirstName,
                c.TrainerLastName
            FROM dbo.Sessions s
            INNER JOIN dbo.Courses c ON c.CourseId = s.CourseId
            LEFT JOIN (
                SELECT SessionId, COUNT(*) AS ParticipantCount
                FROM dbo.Participants
                GROUP BY SessionId
            ) pc ON pc.SessionId = s.SessionId
            WHERE (@Audience IS NULL OR c.CseAudience = @Audience)
              AND (@DeliveryMode IS NULL OR s.DeliveryMode = @DeliveryMode)
              AND (@StartAfter IS NULL OR s.StartDate >= @StartAfter)
              AND (@StartBefore IS NULL OR s.StartDate <= @StartBefore)
              AND (@StartFrom IS NULL OR s.StartDate >= @StartFrom)
              AND (@StartTo IS NULL OR s.StartDate <= @StartTo)
            ORDER BY s.StartDate;
            ";
        await using var conn = await connectionFactory.OpenConnectionAsync(cancellationToken);
        var rows = await conn.QueryAsync<PublicSessionListRead>(
            new CommandDefinition(
                commandText: sql,
                parameters: new
                {
                    Audience = criteria.Audience.HasValue ? (byte?)criteria.Audience.Value : null,
                    DeliveryMode = criteria.DeliveryMode.HasValue ? (byte?)criteria.DeliveryMode.Value : null,
                    StartAfter = criteria.StartAfter,
                    StartBefore = criteria.StartBefore,
                    StartFrom = criteria.StartFrom,
                    StartTo = criteria.StartTo,
                },
                cancellationToken: cancellationToken));
        return [.. rows.Select(ToDomain)];
    }

    public async Task<PublicSessionCatalogDetail?> GetPublicDetailAsync(int sessionId, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            SELECT
                s.SessionId,
                c.Name AS CourseName,
                c.ShortDescription,
                c.LongDescription AS LongDescriptionMarkdown,
                c.CseAudience,
                s.StartDate,
                c.DurationDays,
                s.DeliveryMode,
                CASE
                    WHEN c.MaxCapacity - ISNULL(pc.ParticipantCount, 0) < 0 THEN 0
                    ELSE c.MaxCapacity - ISNULL(pc.ParticipantCount, 0)
                END AS RemainingSeats,
                c.TrainerFirstName,
                c.TrainerLastName
            FROM dbo.Sessions s
            INNER JOIN dbo.Courses c ON c.CourseId = s.CourseId
            LEFT JOIN (
                SELECT SessionId, COUNT(*) AS ParticipantCount
                FROM dbo.Participants
                GROUP BY SessionId
            ) pc ON pc.SessionId = s.SessionId
            WHERE s.SessionId = @SessionId;
            ";
        await using var conn = await connectionFactory.OpenConnectionAsync(cancellationToken);
        var row = await conn.QuerySingleOrDefaultAsync<PublicSessionDetailRead>(
            new CommandDefinition(commandText: sql, parameters: new { SessionId = sessionId }, cancellationToken: cancellationToken));
        return row is null ? null : ToDomain(row);
    }

    public async Task<IReadOnlyList<AdminSessionOverview>> ListAdminAsync(CancellationToken cancellationToken = default)
    {
        const string sql = @"
            SELECT
                s.SessionId,
                s.CourseId,
                c.Name AS CourseName,
                s.StartDate,
                s.DeliveryMode,
                ISNULL(pc.ParticipantCount, 0) AS ParticipantCount,
                c.MaxCapacity
            FROM dbo.Sessions s
            INNER JOIN dbo.Courses c ON c.CourseId = s.CourseId
            LEFT JOIN (
                SELECT SessionId, COUNT(*) AS ParticipantCount
                FROM dbo.Participants
                GROUP BY SessionId
            ) pc ON pc.SessionId = s.SessionId
            ORDER BY s.StartDate DESC;
            ";
        await using var conn = await connectionFactory.OpenConnectionAsync(cancellationToken);
        var rows = await conn.QueryAsync<AdminSessionRead>(
            new CommandDefinition(commandText: sql, parameters: null, cancellationToken: cancellationToken));
        return [.. rows.Select(ToDomain)];
    }

    public async Task<AdminSessionOverview?> GetAdminByIdAsync(int sessionId, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            SELECT
                s.SessionId,
                s.CourseId,
                c.Name AS CourseName,
                s.StartDate,
                s.DeliveryMode,
                ISNULL(pc.ParticipantCount, 0) AS ParticipantCount,
                c.MaxCapacity
            FROM dbo.Sessions s
            INNER JOIN dbo.Courses c ON c.CourseId = s.CourseId
            LEFT JOIN (
                SELECT SessionId, COUNT(*) AS ParticipantCount
                FROM dbo.Participants
                GROUP BY SessionId
            ) pc ON pc.SessionId = s.SessionId
            WHERE s.SessionId = @SessionId;
            ";
        await using var conn = await connectionFactory.OpenConnectionAsync(cancellationToken);
        var row = await conn.QuerySingleOrDefaultAsync<AdminSessionRead>(
            new CommandDefinition(commandText: sql, parameters: new { SessionId = sessionId }, cancellationToken: cancellationToken));
        return row is null ? null : ToDomain(row);
    }

    public async Task<int> InsertAsync(int courseId, DateTime startDate, SessionDeliveryMode deliveryMode, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            INSERT INTO dbo.Sessions (CourseId, StartDate, DeliveryMode)
            OUTPUT INSERTED.SessionId
            VALUES (@CourseId, @StartDate, @DeliveryMode);
            ";
        await using var conn = await connectionFactory.OpenConnectionAsync(cancellationToken);
        return await conn.QuerySingleAsync<int>(
            new CommandDefinition(
                commandText: sql,
                parameters: new { CourseId = courseId, StartDate = startDate, DeliveryMode = (byte)deliveryMode },
                cancellationToken: cancellationToken));
    }

    public async Task<bool> UpdateAsync(int sessionId, int courseId, DateTime startDate, SessionDeliveryMode deliveryMode, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            UPDATE dbo.Sessions
            SET CourseId = @CourseId,
                StartDate = @StartDate,
                DeliveryMode = @DeliveryMode
            WHERE SessionId = @SessionId;
            ";
        await using var conn = await connectionFactory.OpenConnectionAsync(cancellationToken);
        var affected = await conn.ExecuteAsync(
            new CommandDefinition(
                commandText: sql,
                parameters: new
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
        const string sql = @"
            DELETE FROM dbo.Sessions
            WHERE SessionId = @SessionId;
            ";
        await using var conn = await connectionFactory.OpenConnectionAsync(cancellationToken);
        await conn.ExecuteAsync(
            new CommandDefinition(commandText: sql, parameters: new { SessionId = sessionId }, cancellationToken: cancellationToken));
    }

    private static PublicSessionListing ToDomain(PublicSessionListRead r) =>
        new(
            r.SessionId,
            r.CourseName,
            r.ShortDescription,
            r.CseAudience,
            r.StartDate,
            r.DurationDays,
            r.DeliveryMode,
            r.RemainingSeats,
            r.TrainerFirstName,
            r.TrainerLastName);

    private static PublicSessionCatalogDetail ToDomain(PublicSessionDetailRead r) =>
        new(
            r.SessionId,
            r.CourseName,
            r.ShortDescription,
            r.LongDescriptionMarkdown,
            r.CseAudience,
            r.StartDate,
            r.DurationDays,
            r.DeliveryMode,
            r.RemainingSeats,
            r.TrainerFirstName,
            r.TrainerLastName);

    private static AdminSessionOverview ToDomain(AdminSessionRead r) =>
        new(
            r.SessionId,
            r.CourseId,
            r.CourseName,
            r.StartDate,
            r.DeliveryMode,
            r.ParticipantCount,
            r.MaxCapacity);
}
