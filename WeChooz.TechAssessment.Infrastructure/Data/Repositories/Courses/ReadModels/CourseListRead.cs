using WeChooz.TechAssessment.Domain.Courses;

namespace WeChooz.TechAssessment.Infrastructure.Data.Repositories.Courses.ReadModels;

internal sealed record CourseListRead(int CourseId, string Name, CseAudience CseAudience, int DurationDays, int MaxCapacity);
