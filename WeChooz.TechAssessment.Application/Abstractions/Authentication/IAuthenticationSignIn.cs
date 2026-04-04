using System.Security.Claims;

namespace WeChooz.TechAssessment.Application.Abstractions.Authentication;

public interface IAuthenticationSignIn
{
    Task SignInAsync(ClaimsPrincipal principal, CancellationToken cancellationToken = default);
}
