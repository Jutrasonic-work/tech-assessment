using WeChooz.TechAssessment.Domain.Courses;

namespace WeChooz.TechAssessment.Domain.ReadModels;

public sealed record CourseListItemResult(int CourseId, string Name, CseAudience CseAudience, int DurationDays, int MaxCapacity);

public sealed record CourseDetailResult(
    int CourseId,
    string Name,
    string ShortDescription,
    string LongDescriptionMarkdown,
    int DurationDays,
    CseAudience CseAudience,
    int MaxCapacity,
    string TrainerFirstName,
    string TrainerLastName);
