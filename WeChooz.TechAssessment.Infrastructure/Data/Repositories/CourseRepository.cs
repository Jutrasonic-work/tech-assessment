using Dapper;
using Microsoft.Data.SqlClient;
using WeChooz.TechAssessment.Application.Abstractions.Data;
using WeChooz.TechAssessment.Domain.Courses;
using WeChooz.TechAssessment.Domain.ReadModels;
using WeChooz.TechAssessment.Domain.Repositories;
using WeChooz.TechAssessment.Infrastructure.Data.Sql;

namespace WeChooz.TechAssessment.Infrastructure.Data.Repositories;

public sealed class CourseRepository(IDbConnectionFactory connectionFactory) : ICourseRepository
{
    public async Task<IReadOnlyList<CourseListItemResult>> ListAsync(CancellationToken cancellationToken = default)
    {
        await using var conn = await OpenAsync(cancellationToken);
        var rows = await conn.QueryAsync<CourseListItemResult>(new CommandDefinition(CourseSql.List, cancellationToken: cancellationToken));
        return rows.AsList();
    }

    public async Task<CourseDetailResult?> GetByIdAsync(int courseId, CancellationToken cancellationToken = default)
    {
        await using var conn = await OpenAsync(cancellationToken);
        return await conn.QuerySingleOrDefaultAsync<CourseDetailResult>(
            new CommandDefinition(CourseSql.GetById, new { CourseId = courseId }, cancellationToken: cancellationToken));
    }

    public async Task<int> InsertAsync(
        string name,
        string shortDescription,
        string longDescriptionMarkdown,
        int durationDays,
        CseAudience cseAudience,
        int maxCapacity,
        string trainerFirstName,
        string trainerLastName,
        CancellationToken cancellationToken = default)
    {
        await using var conn = await OpenAsync(cancellationToken);
        return await conn.QuerySingleAsync<int>(
            new CommandDefinition(
                CourseSql.Insert,
                new
                {
                    Name = name,
                    ShortDescription = shortDescription,
                    LongDescription = longDescriptionMarkdown,
                    DurationDays = durationDays,
                    CseAudience = (byte)cseAudience,
                    MaxCapacity = maxCapacity,
                    TrainerFirstName = trainerFirstName,
                    TrainerLastName = trainerLastName,
                },
                cancellationToken: cancellationToken));
    }

    public async Task<bool> UpdateAsync(
        int courseId,
        string name,
        string shortDescription,
        string longDescriptionMarkdown,
        int durationDays,
        CseAudience cseAudience,
        int maxCapacity,
        string trainerFirstName,
        string trainerLastName,
        CancellationToken cancellationToken = default)
    {
        await using var conn = await OpenAsync(cancellationToken);
        var affected = await conn.ExecuteAsync(
            new CommandDefinition(
                CourseSql.Update,
                new
                {
                    CourseId = courseId,
                    Name = name,
                    ShortDescription = shortDescription,
                    LongDescription = longDescriptionMarkdown,
                    DurationDays = durationDays,
                    CseAudience = (byte)cseAudience,
                    MaxCapacity = maxCapacity,
                    TrainerFirstName = trainerFirstName,
                    TrainerLastName = trainerLastName,
                },
                cancellationToken: cancellationToken));
        return affected > 0;
    }

    public async Task DeleteAsync(int courseId, CancellationToken cancellationToken = default)
    {
        await using var conn = await OpenAsync(cancellationToken);
        await conn.ExecuteAsync(new CommandDefinition(CourseSql.Delete, new { CourseId = courseId }, cancellationToken: cancellationToken));
    }

    private async Task<SqlConnection> OpenAsync(CancellationToken cancellationToken)
    {
        var conn = (SqlConnection)connectionFactory.CreateConnection();
        await conn.OpenAsync(cancellationToken);
        return conn;
    }
}
