using WeChooz.TechAssessment.Domain.Courses;

namespace WeChooz.TechAssessment.Application.Persistence.Courses;

public interface ICourseRepository
{
    Task<IReadOnlyList<CourseSummary>> ListAsync(CancellationToken cancellationToken = default);

    Task<Course?> GetByIdAsync(int courseId, CancellationToken cancellationToken = default);

    Task<int> InsertAsync(
        string name,
        string shortDescription,
        string longDescriptionMarkdown,
        int durationDays,
        CseAudience cseAudience,
        int maxCapacity,
        string trainerFirstName,
        string trainerLastName,
        CancellationToken cancellationToken = default);

    Task<bool> UpdateAsync(
        int courseId,
        string name,
        string shortDescription,
        string longDescriptionMarkdown,
        int durationDays,
        CseAudience cseAudience,
        int maxCapacity,
        string trainerFirstName,
        string trainerLastName,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(int courseId, CancellationToken cancellationToken = default);
}
