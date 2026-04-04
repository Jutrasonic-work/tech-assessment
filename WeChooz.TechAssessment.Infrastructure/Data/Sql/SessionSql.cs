namespace WeChooz.TechAssessment.Infrastructure.Data.Sql;

internal static class SessionSql
{
    /// <summary>Filtres date : combinaison AND (après, avant, entre).</summary>
    internal const string ListPublic = """
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
        """;

    internal const string GetPublicDetail = """
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
        """;

    internal const string ListAdmin = """
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
        """;

    internal const string GetAdminById = """
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
        """;

    internal const string Insert = """
        INSERT INTO dbo.Sessions (CourseId, StartDate, DeliveryMode)
        OUTPUT INSERTED.SessionId
        VALUES (@CourseId, @StartDate, @DeliveryMode);
        """;

    internal const string Update = """
        UPDATE dbo.Sessions
        SET CourseId = @CourseId,
            StartDate = @StartDate,
            DeliveryMode = @DeliveryMode
        WHERE SessionId = @SessionId;
        """;

    internal const string Delete = """
        DELETE FROM dbo.Sessions
        WHERE SessionId = @SessionId;
        """;
}
