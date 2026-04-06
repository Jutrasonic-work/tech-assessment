namespace WeChooz.TechAssessment.Domain.Courses;

public sealed record CourseSummary(int CourseId, string Name, CseAudience CseAudience, int DurationDays, int MaxCapacity);
