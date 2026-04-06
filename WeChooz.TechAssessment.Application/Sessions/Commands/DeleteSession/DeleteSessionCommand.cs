using Shared.Mediator;

namespace WeChooz.TechAssessment.Application.Sessions.Commands.DeleteSession;

public sealed record DeleteSessionCommand(int SessionId) : IRequest;
