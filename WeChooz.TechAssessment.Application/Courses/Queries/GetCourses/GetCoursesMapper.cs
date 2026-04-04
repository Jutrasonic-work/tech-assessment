using WeChooz.TechAssessment.Domain.Courses;

namespace WeChooz.TechAssessment.Application.Courses.Queries.GetCourses;

internal static class GetCoursesMapper
{
    public static GetCoursesItem ToItem(this CourseSummary c) =>
        new(c.CourseId, c.Name, c.CseAudience, c.DurationDays, c.MaxCapacity);
}
