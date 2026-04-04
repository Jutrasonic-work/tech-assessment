using WeChooz.TechAssessment.Domain.Participants;

namespace WeChooz.TechAssessment.Application.Participants.Queries.GetParticipantsBySession;

internal static class GetParticipantsBySessionMapper
{
    public static GetParticipantsBySessionItem ToItem(this Participant p) =>
        new(p.ParticipantId, p.SessionId, p.Name.LastName, p.Name.FirstName, p.Email.Value, p.CompanyName);
}
