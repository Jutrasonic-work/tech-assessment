using System.Security.Claims;
using Shared.Mediator;
using WeChooz.TechAssessment.Application.Interfaces.Authentication;

namespace WeChooz.TechAssessment.Application.Auth.Commands.Login;

public sealed class LoginHandler(IAuthenticationSignIn signIn) : IRequestHandler<LoginCommand, LoginResult>
{
    private const string CookieAuthenticationScheme = "Cookies";
    public async Task<LoginResult> HandleAsync(LoginCommand request, CancellationToken cancellationToken = default)
    {
        if (request.Login is not ("formation" or "sales"))
        {
            return new LoginResult(null, LoginFailureKind.InvalidCredentials);
        }

        var principal = new ClaimsPrincipal([
            new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.Role, request.Login),
                    new Claim(ClaimTypes.Name, request.Login),
                ],
                authenticationType: CookieAuthenticationScheme),
        ]);

        await signIn.SignInAsync(principal, cancellationToken);
        var claims = principal.Claims.Select(c => new LoginClaimDto(c.Type, c.Value)).ToList();
        return new LoginResult(new LoginResponse(request.Login, claims), null);
    }
}
