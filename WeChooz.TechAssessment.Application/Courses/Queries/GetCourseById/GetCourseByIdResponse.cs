using WeChooz.TechAssessment.Domain.Courses;

namespace WeChooz.TechAssessment.Application.Courses.Queries.GetCourseById;

public sealed record GetCourseByIdResponse(
    int CourseId,
    string Name,
    string ShortDescription,
    string LongDescriptionMarkdown,
    int DurationDays,
    CseAudience CseAudience,
    int MaxCapacity,
    string TrainerFirstName,
    string TrainerLastName);
