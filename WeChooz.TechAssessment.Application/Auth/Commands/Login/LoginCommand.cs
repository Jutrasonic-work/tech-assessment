using Shared.Mediator;

namespace WeChooz.TechAssessment.Application.Auth.Commands.Login;

public sealed record LoginCommand(string Login) : IRequest<LoginResult>;
