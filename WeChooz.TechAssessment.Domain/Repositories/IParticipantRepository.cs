using WeChooz.TechAssessment.Domain.ReadModels;

namespace WeChooz.TechAssessment.Domain.Repositories;

public interface IParticipantRepository
{
    Task<IReadOnlyList<ParticipantResult>> ListBySessionAsync(int sessionId, CancellationToken cancellationToken = default);

    Task<int> InsertAsync(
        int sessionId,
        string lastName,
        string firstName,
        string email,
        string companyName,
        CancellationToken cancellationToken = default);

    Task<bool> UpdateAsync(
        int participantId,
        int sessionId,
        string lastName,
        string firstName,
        string email,
        string companyName,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(int participantId, CancellationToken cancellationToken = default);
}
