using System.Security.Claims;
using Shared.Mediator.Application;
using WeChooz.TechAssessment.Application.Abstractions.Authentication;

namespace WeChooz.TechAssessment.Application.Auth.Commands.Login;

public sealed class LoginHandler(IAuthenticationSignIn signIn) : IRequestHandler<LoginCommand, LoginResult>
{
    public async Task<LoginResult> HandleAsync(LoginCommand request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Login))
        {
            return new LoginResult(null, LoginFailureKind.EmptyLogin);
        }

        if (request.Login is not ("formation" or "sales"))
        {
            return new LoginResult(null, LoginFailureKind.InvalidCredentials);
        }

        var principal = new ClaimsPrincipal([
            new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.Role, request.Login),
                    new Claim(ClaimTypes.Name, request.Login),
                ]),
        ]);

        await signIn.SignInAsync(principal, cancellationToken);
        var claims = principal.Claims.Select(c => new LoginClaimDto(c.Type, c.Value)).ToList();
        return new LoginResult(new LoginResponse(request.Login, claims), null);
    }
}
