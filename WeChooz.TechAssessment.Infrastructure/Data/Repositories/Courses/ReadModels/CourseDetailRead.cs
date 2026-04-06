using WeChooz.TechAssessment.Domain.Courses;

namespace WeChooz.TechAssessment.Infrastructure.Data.Repositories.Courses.ReadModels;

internal sealed record CourseDetailRead(
    int CourseId,
    string Name,
    string ShortDescription,
    string LongDescriptionMarkdown,
    int DurationDays,
    CseAudience CseAudience,
    int MaxCapacity,
    string TrainerFirstName,
    string TrainerLastName);
