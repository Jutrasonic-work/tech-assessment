using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using WeChooz.TechAssessment.Application.Interfaces.Authentication;

namespace WeChooz.TechAssessment.Web.Authentication;

public sealed class HttpContextAuthenticationSignIn(IHttpContextAccessor httpContextAccessor) : IAuthenticationSignIn
{
    public Task SignInAsync(ClaimsPrincipal principal, CancellationToken cancellationToken = default)
    {
        var httpContext = httpContextAccessor.HttpContext ?? throw new InvalidOperationException("HttpContext indisponible.");
        return httpContext.SignInAsync(principal);
    }
}
