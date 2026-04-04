using Shared.Mediator.Application;

namespace WeChooz.TechAssessment.Application.Participants.Commands.RemoveParticipant;

public sealed record RemoveParticipantCommand(int ParticipantId) : IRequest;
