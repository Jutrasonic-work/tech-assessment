using Shared.Mediator.Application;

namespace WeChooz.TechAssessment.Application.Participants.Commands.AddParticipant;

public sealed record AddParticipantCommand(
    int SessionId,
    string LastName,
    string FirstName,
    string Email,
    string CompanyName) : IRequest<AddParticipantResponse>;
