using Shared.Mediator.Application;

namespace WeChooz.TechAssessment.Application.Participants.Queries.GetParticipantsBySession;

public sealed record GetParticipantsBySessionQuery(int SessionId) : IRequest<GetParticipantsBySessionResponse>;
