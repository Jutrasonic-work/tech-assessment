using Dapper;
using Microsoft.Data.SqlClient;
using WeChooz.TechAssessment.Application.Abstractions.Data;
using WeChooz.TechAssessment.Domain.ReadModels;
using WeChooz.TechAssessment.Domain.Repositories;
using WeChooz.TechAssessment.Infrastructure.Data.Sql;

namespace WeChooz.TechAssessment.Infrastructure.Data.Repositories;

public sealed class ParticipantRepository(IDbConnectionFactory connectionFactory) : IParticipantRepository
{
    public async Task<IReadOnlyList<ParticipantResult>> ListBySessionAsync(int sessionId, CancellationToken cancellationToken = default)
    {
        await using var conn = await OpenAsync(cancellationToken);
        var rows = await conn.QueryAsync<ParticipantResult>(
            new CommandDefinition(ParticipantSql.ListBySession, new { SessionId = sessionId }, cancellationToken: cancellationToken));
        return rows.AsList();
    }

    public async Task<int> InsertAsync(
        int sessionId,
        string lastName,
        string firstName,
        string email,
        string companyName,
        CancellationToken cancellationToken = default)
    {
        await using var conn = await OpenAsync(cancellationToken);
        return await conn.QuerySingleAsync<int>(
            new CommandDefinition(
                ParticipantSql.Insert,
                new
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
        await using var conn = await OpenAsync(cancellationToken);
        var affected = await conn.ExecuteAsync(
            new CommandDefinition(
                ParticipantSql.Update,
                new
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
        await using var conn = await OpenAsync(cancellationToken);
        await conn.ExecuteAsync(new CommandDefinition(ParticipantSql.Delete, new { ParticipantId = participantId }, cancellationToken: cancellationToken));
    }

    private async Task<SqlConnection> OpenAsync(CancellationToken cancellationToken)
    {
        var conn = (SqlConnection)connectionFactory.CreateConnection();
        await conn.OpenAsync(cancellationToken);
        return conn;
    }
}
