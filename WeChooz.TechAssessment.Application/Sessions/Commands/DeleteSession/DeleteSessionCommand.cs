using Shared.Mediator.Application;

namespace WeChooz.TechAssessment.Application.Sessions.Commands.DeleteSession;

public sealed record DeleteSessionCommand(int SessionId) : IRequest;
