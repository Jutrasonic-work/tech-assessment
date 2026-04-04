using WeChooz.TechAssessment.Domain.Courses;

namespace WeChooz.TechAssessment.Application.Courses.Queries.GetCourseById;

internal static class GetCourseByIdMapper
{
    public static GetCourseByIdResponse ToResponse(this Course c) =>
        new(
            c.CourseId,
            c.Name,
            c.ShortDescription,
            c.LongDescription,
            c.DurationDays,
            c.CseAudience,
            c.MaxCapacity,
            c.Trainer.FirstName,
            c.Trainer.LastName);
}
