using WeChooz.TechAssessment.Domain.Courses;
using WeChooz.TechAssessment.Domain.ReadModels;

namespace WeChooz.TechAssessment.Domain.Repositories;

public interface ICourseRepository
{
    Task<IReadOnlyList<CourseListItemResult>> ListAsync(CancellationToken cancellationToken = default);

    Task<CourseDetailResult?> GetByIdAsync(int courseId, CancellationToken cancellationToken = default);

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
