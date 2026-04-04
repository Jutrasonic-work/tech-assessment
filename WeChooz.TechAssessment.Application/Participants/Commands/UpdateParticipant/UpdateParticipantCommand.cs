using Shared.Mediator.Application;

namespace WeChooz.TechAssessment.Application.Participants.Commands.UpdateParticipant;

public sealed record UpdateParticipantCommand(
    int ParticipantId,
    int SessionId,
    string LastName,
    string FirstName,
    string Email,
    string CompanyName) : IRequest<UpdateParticipantResponse>;
