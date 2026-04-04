using WeChooz.TechAssessment.Domain.ReadModels;

namespace WeChooz.TechAssessment.Application.Courses.Queries.GetCourseById;

internal static class GetCourseByIdMapper
{
    public static GetCourseByIdResponse ToResponse(this CourseDetailResult r) =>
        new(
            r.CourseId,
            r.Name,
            r.ShortDescription,
            r.LongDescriptionMarkdown,
            r.DurationDays,
            r.CseAudience,
            r.MaxCapacity,
            r.TrainerFirstName,
            r.TrainerLastName);
}
