namespace WeChooz.TechAssessment.Domain.Courses;

/// <summary>Aperçu d'un cours pour listes (catalogue admin).</summary>
public sealed record CourseSummary(int CourseId, string Name, CseAudience CseAudience, int DurationDays, int MaxCapacity);
