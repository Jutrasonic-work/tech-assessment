namespace WeChooz.TechAssessment.Application.Participants.Queries.GetParticipantsBySession;

public sealed record GetParticipantsBySessionResponse(IReadOnlyList<GetParticipantsBySessionItem> Items);

public sealed record GetParticipantsBySessionItem(int ParticipantId, int SessionId, string LastName, string FirstName, string Email, string CompanyName);
