using Shared.Mediator.Application;

namespace WeChooz.TechAssessment.Application.Auth.Commands.Login;

public sealed record LoginCommand(string Login) : IRequest<LoginResult>;
