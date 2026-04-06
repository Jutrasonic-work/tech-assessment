using Shared.Mediator;
using WeChooz.TechAssessment.Domain.Courses;

namespace WeChooz.TechAssessment.Application.Courses.Commands.UpdateCourse;

public sealed record UpdateCourseCommand(
    int CourseId,
    string Name,
    string ShortDescription,
    string LongDescriptionMarkdown,
    int DurationDays,
    CseAudience CseAudience,
    int MaxCapacity,
    string TrainerFirstName,
    string TrainerLastName) : IRequest<UpdateCourseResponse>;
