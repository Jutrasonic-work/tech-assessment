using Shared.Mediator;

namespace WeChooz.TechAssessment.Application.Participants.Commands.RemoveParticipant;

public sealed record RemoveParticipantCommand(int ParticipantId) : IRequest;
