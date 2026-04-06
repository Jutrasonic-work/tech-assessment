using WeChooz.TechAssessment.Domain.Courses;

namespace WeChooz.TechAssessment.Application.Courses.Queries.GetCourses;

public sealed record GetCoursesResponse(IReadOnlyList<GetCoursesItem> Items);

public sealed record GetCoursesItem(int CourseId, string Name, CseAudience CseAudience, int DurationDays, int MaxCapacity);
