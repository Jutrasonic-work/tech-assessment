using WeChooz.TechAssessment.Domain.Sessions;

namespace WeChooz.TechAssessment.Application.Interfaces.Sessions;

public interface ISessionRepository
{
    Task<IReadOnlyList<PublicSessionListing>> ListPublicAsync(PublicSessionListCriteria criteria, CancellationToken cancellationToken = default);

    Task<PublicSessionCatalogDetail?> GetPublicDetailAsync(int sessionId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AdminSessionOverview>> ListAdminAsync(CancellationToken cancellationToken = default);

    Task<AdminSessionOverview?> GetAdminByIdAsync(int sessionId, CancellationToken cancellationToken = default);

    Task<int> InsertAsync(int courseId, DateTime startDate, SessionDeliveryMode deliveryMode, CancellationToken cancellationToken = default);

    Task<bool> UpdateAsync(int sessionId, int courseId, DateTime startDate, SessionDeliveryMode deliveryMode, CancellationToken cancellationToken = default);

    Task DeleteAsync(int sessionId, CancellationToken cancellationToken = default);
}
