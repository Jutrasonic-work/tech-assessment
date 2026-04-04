using WeChooz.TechAssessment.Domain.Participants;

namespace WeChooz.TechAssessment.Application.Persistence.Participants;

public interface IParticipantRepository
{
    Task<IReadOnlyList<Participant>> ListBySessionAsync(int sessionId, CancellationToken cancellationToken = default);

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
