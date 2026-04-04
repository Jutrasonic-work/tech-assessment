using WeChooz.TechAssessment.Domain.ReadModels;

namespace WeChooz.TechAssessment.Application.Courses.Queries.GetCourses;

internal static class GetCoursesMapper
{
    public static GetCoursesItem ToItem(this CourseListItemResult r) =>
        new(r.CourseId, r.Name, r.CseAudience, r.DurationDays, r.MaxCapacity);
}
