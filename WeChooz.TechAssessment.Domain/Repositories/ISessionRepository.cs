using WeChooz.TechAssessment.Domain.ReadModels;
using WeChooz.TechAssessment.Domain.Sessions;

namespace WeChooz.TechAssessment.Domain.Repositories;

public interface ISessionRepository
{
    Task<IReadOnlyList<PublicSessionListItemResult>> ListPublicAsync(ListPublicSessionsFilter filter, CancellationToken cancellationToken = default);

    Task<PublicSessionDetailResult?> GetPublicDetailAsync(int sessionId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AdminSessionListItemResult>> ListAdminAsync(CancellationToken cancellationToken = default);

    Task<AdminSessionDetailResult?> GetAdminByIdAsync(int sessionId, CancellationToken cancellationToken = default);

    Task<int> InsertAsync(int courseId, DateTime startDate, SessionDeliveryMode deliveryMode, CancellationToken cancellationToken = default);

    Task<bool> UpdateAsync(int sessionId, int courseId, DateTime startDate, SessionDeliveryMode deliveryMode, CancellationToken cancellationToken = default);

    Task DeleteAsync(int sessionId, CancellationToken cancellationToken = default);
}
