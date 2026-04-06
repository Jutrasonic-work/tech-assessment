using Dapper;
using WeChooz.TechAssessment.Application.Interfaces.Participants;
using WeChooz.TechAssessment.Domain.Common;
using WeChooz.TechAssessment.Domain.Participants;
using WeChooz.TechAssessment.Infrastructure.Data.Repositories.Participants.ReadModels;

namespace WeChooz.TechAssessment.Infrastructure.Data.Repositories.Participants;

public sealed class ParticipantRepository(IDbConnectionFactory connectionFactory) : IParticipantRepository
{
    public async Task<IReadOnlyList<Participant>> ListBySessionAsync(int sessionId, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            SELECT ParticipantId, SessionId, LastName, FirstName, Email, CompanyName
            FROM dbo.Participants
            WHERE SessionId = @SessionId
            ORDER BY LastName, FirstName;
            ";
        await using var conn = await connectionFactory.OpenConnectionAsync(cancellationToken);
        var rows = await conn.QueryAsync<ParticipantRead>(
            new CommandDefinition(commandText: sql, parameters: new { SessionId = sessionId }, cancellationToken: cancellationToken));
        return [.. rows.Select(ToDomain)];
    }

    public async Task<int> InsertAsync(
        int sessionId,
        string lastName,
        string firstName,
        string email,
        string companyName,
        CancellationToken cancellationToken = default)
    {
        const string sql = @"
            INSERT INTO dbo.Participants (SessionId, LastName, FirstName, Email, CompanyName)
            OUTPUT INSERTED.ParticipantId
            VALUES (@SessionId, @LastName, @FirstName, @Email, @CompanyName);
            ";
        await using var conn = await connectionFactory.OpenConnectionAsync(cancellationToken);
        return await conn.QuerySingleAsync<int>(
            new CommandDefinition(
                commandText: sql,
                parameters: new
                {
                    SessionId = sessionId,
                    LastName = lastName,
                    FirstName = firstName,
                    Email = email,
                    CompanyName = companyName,
                },
                cancellationToken: cancellationToken));
    }

    public async Task<bool> UpdateAsync(
        int participantId,
        int sessionId,
        string lastName,
        string firstName,
        string email,
        string companyName,
        CancellationToken cancellationToken = default)
    {
        const string sql = @"
            UPDATE dbo.Participants
            SET LastName = @LastName,
                FirstName = @FirstName,
                Email = @Email,
                CompanyName = @CompanyName
            WHERE ParticipantId = @ParticipantId
              AND SessionId = @SessionId;
            ";
        await using var conn = await connectionFactory.OpenConnectionAsync(cancellationToken);
        var affected = await conn.ExecuteAsync(
            new CommandDefinition(
                commandText: sql,
                parameters: new
                {
                    ParticipantId = participantId,
                    SessionId = sessionId,
                    LastName = lastName,
                    FirstName = firstName,
                    Email = email,
                    CompanyName = companyName,
                },
                cancellationToken: cancellationToken));
        return affected > 0;
    }

    public async Task DeleteAsync(int participantId, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            DELETE FROM dbo.Participants
            WHERE ParticipantId = @ParticipantId;
            ";
        await using var conn = await connectionFactory.OpenConnectionAsync(cancellationToken);
        await conn.ExecuteAsync(
            new CommandDefinition(commandText: sql, parameters: new { ParticipantId = participantId }, cancellationToken: cancellationToken));
    }

    private static Participant ToDomain(ParticipantRead r) =>
        new(
            r.ParticipantId,
            r.SessionId,
            new PersonName(r.FirstName, r.LastName),
            new EmailAddress(r.Email),
            r.CompanyName);
}
