using Shared.Mediator;
using WeChooz.TechAssessment.Domain.Courses;

namespace WeChooz.TechAssessment.Application.Courses.Commands.CreateCourse;

public sealed record CreateCourseCommand(
    string Name,
    string ShortDescription,
    string LongDescriptionMarkdown,
    int DurationDays,
    CseAudience CseAudience,
    int MaxCapacity,
    string TrainerFirstName,
    string TrainerLastName) : IRequest<CreateCourseResponse>;
