using System.Security.Claims;

namespace WeChooz.TechAssessment.Application.Interfaces.Authentication;

public interface IAuthenticationSignIn
{
    Task SignInAsync(ClaimsPrincipal principal, CancellationToken cancellationToken = default);
}
