using Dapper;
using WeChooz.TechAssessment.Application.Persistence.Courses;
using WeChooz.TechAssessment.Domain.Common;
using WeChooz.TechAssessment.Domain.Courses;
using WeChooz.TechAssessment.Infrastructure.Data;
using WeChooz.TechAssessment.Infrastructure.Data.Repositories.Courses.ReadModels;

namespace WeChooz.TechAssessment.Infrastructure.Data.Repositories.Courses;

public sealed class CourseRepository(IDbConnectionFactory connectionFactory) : ICourseRepository
{
    public async Task<IReadOnlyList<CourseSummary>> ListAsync(CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT CourseId, Name, CseAudience, DurationDays, MaxCapacity
            FROM dbo.Courses
            ORDER BY Name;
            """;
        await using var conn = await connectionFactory.OpenConnectionAsync(cancellationToken);
        var rows = await conn.QueryAsync<CourseListRow>(
            new CommandDefinition(commandText: sql, parameters: null, cancellationToken: cancellationToken));
        return rows.Select(ToDomain).ToList();
    }

    public async Task<Course?> GetByIdAsync(int courseId, CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT CourseId, Name, ShortDescription, LongDescription AS LongDescriptionMarkdown, DurationDays, CseAudience, MaxCapacity,
                   TrainerFirstName, TrainerLastName
            FROM dbo.Courses
            WHERE CourseId = @CourseId;
            """;
        await using var conn = await connectionFactory.OpenConnectionAsync(cancellationToken);
        var row = await conn.QuerySingleOrDefaultAsync<CourseDetailRow>(
            new CommandDefinition(commandText: sql, parameters: new { CourseId = courseId }, cancellationToken: cancellationToken));
        return row is null ? null : ToDomain(row);
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
        const string sql = """
            INSERT INTO dbo.Courses (Name, ShortDescription, LongDescription, DurationDays, CseAudience, MaxCapacity, TrainerFirstName, TrainerLastName)
            OUTPUT INSERTED.CourseId
            VALUES (@Name, @ShortDescription, @LongDescription, @DurationDays, @CseAudience, @MaxCapacity, @TrainerFirstName, @TrainerLastName);
            """;
        await using var conn = await connectionFactory.OpenConnectionAsync(cancellationToken);
        return await conn.QuerySingleAsync<int>(
            new CommandDefinition(
                commandText: sql,
                parameters: new
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
        const string sql = """
            UPDATE dbo.Courses
            SET Name = @Name,
                ShortDescription = @ShortDescription,
                LongDescription = @LongDescription,
                DurationDays = @DurationDays,
                CseAudience = @CseAudience,
                MaxCapacity = @MaxCapacity,
                TrainerFirstName = @TrainerFirstName,
                TrainerLastName = @TrainerLastName
            WHERE CourseId = @CourseId;
            """;
        await using var conn = await connectionFactory.OpenConnectionAsync(cancellationToken);
        var affected = await conn.ExecuteAsync(
            new CommandDefinition(
                commandText: sql,
                parameters: new
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
        const string sql = """
            DELETE FROM dbo.Courses
            WHERE CourseId = @CourseId;
            """;
        await using var conn = await connectionFactory.OpenConnectionAsync(cancellationToken);
        await conn.ExecuteAsync(
            new CommandDefinition(commandText: sql, parameters: new { CourseId = courseId }, cancellationToken: cancellationToken));
    }

    private static CourseSummary ToDomain(CourseListRow r) =>
        new(r.CourseId, r.Name, r.CseAudience, r.DurationDays, r.MaxCapacity);

    private static Course ToDomain(CourseDetailRow r) =>
        new(
            r.CourseId,
            r.Name,
            r.ShortDescription,
            r.LongDescriptionMarkdown,
            r.DurationDays,
            r.CseAudience,
            r.MaxCapacity,
            new PersonName(r.TrainerFirstName, r.TrainerLastName));
}
