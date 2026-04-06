using Shared.Mediator;

namespace WeChooz.TechAssessment.Application.Participants.Queries.GetParticipantsBySession;

public sealed record GetParticipantsBySessionQuery(int SessionId) : IRequest<GetParticipantsBySessionResponse>;
