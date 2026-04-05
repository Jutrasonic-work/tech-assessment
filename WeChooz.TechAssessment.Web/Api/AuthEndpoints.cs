using Microsoft.AspNetCore.Http.HttpResults;
using Shared.Mediator.Application;
using WeChooz.TechAssessment.Application.Auth.Commands.Login;

namespace WeChooz.TechAssessment.Web.Api;

internal static class AuthEndpoints
{
    internal static void MapAuthEndpoints(this RouteGroupBuilder root)
    {
        var auth = root.MapGroup("/auth").AllowAnonymous();

        auth.MapPost("/login", async Task<Results<Ok<LoginResponse>, UnauthorizedHttpResult>> (
            IMediator mediator,
            LoginCommand body,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.SendAsync(body, cancellationToken);
            if (result.Failure == LoginFailureKind.InvalidCredentials)
            {
                return TypedResults.Unauthorized();
            }

            return TypedResults.Ok(result.Response!);
        })
        .WithTags("Authentification")
        .WithSummary("Connexion (cookie)")
        .WithDescription("Valeurs de test : formation ou sales. Pose un cookie d'authentification.");
    }
}
