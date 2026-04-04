namespace WeChooz.TechAssessment.Infrastructure.Data.Sql;

internal static class ParticipantSql
{
    internal const string ListBySession = """
        SELECT ParticipantId, SessionId, LastName, FirstName, Email, CompanyName
        FROM dbo.Participants
        WHERE SessionId = @SessionId
        ORDER BY LastName, FirstName;
        """;

    internal const string Insert = """
        INSERT INTO dbo.Participants (SessionId, LastName, FirstName, Email, CompanyName)
        OUTPUT INSERTED.ParticipantId
        VALUES (@SessionId, @LastName, @FirstName, @Email, @CompanyName);
        """;

    internal const string Update = """
        UPDATE dbo.Participants
        SET LastName = @LastName,
            FirstName = @FirstName,
            Email = @Email,
            CompanyName = @CompanyName
        WHERE ParticipantId = @ParticipantId
          AND SessionId = @SessionId;
        """;

    internal const string Delete = """
        DELETE FROM dbo.Participants
        WHERE ParticipantId = @ParticipantId;
        """;
}
