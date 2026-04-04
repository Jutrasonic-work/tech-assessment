using WeChooz.TechAssessment.Domain.ReadModels;

namespace WeChooz.TechAssessment.Application.Participants.Queries.GetParticipantsBySession;

internal static class GetParticipantsBySessionMapper
{
    public static GetParticipantsBySessionItem ToItem(this ParticipantResult r) =>
        new(r.ParticipantId, r.SessionId, r.LastName, r.FirstName, r.Email, r.CompanyName);
}
