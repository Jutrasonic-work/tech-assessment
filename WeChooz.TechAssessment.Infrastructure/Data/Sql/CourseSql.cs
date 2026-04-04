namespace WeChooz.TechAssessment.Infrastructure.Data.Sql;

internal static class CourseSql
{
    internal const string List = """
        SELECT CourseId, Name, CseAudience, DurationDays, MaxCapacity
        FROM dbo.Courses
        ORDER BY Name;
        """;

    internal const string GetById = """
        SELECT CourseId, Name, ShortDescription, LongDescription AS LongDescriptionMarkdown, DurationDays, CseAudience, MaxCapacity,
               TrainerFirstName, TrainerLastName
        FROM dbo.Courses
        WHERE CourseId = @CourseId;
        """;

    internal const string Insert = """
        INSERT INTO dbo.Courses (Name, ShortDescription, LongDescription, DurationDays, CseAudience, MaxCapacity, TrainerFirstName, TrainerLastName)
        OUTPUT INSERTED.CourseId
        VALUES (@Name, @ShortDescription, @LongDescription, @DurationDays, @CseAudience, @MaxCapacity, @TrainerFirstName, @TrainerLastName);
        """;

    internal const string Update = """
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

    internal const string Delete = """
        DELETE FROM dbo.Courses
        WHERE CourseId = @CourseId;
        """;
}
