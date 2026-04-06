using Microsoft.AspNetCore.Http.HttpResults;
using Shared.Mediator;
using WeChooz.TechAssessment.Application.Auth.Commands.Login;

namespace WeChooz.TechAssessment.Web.Api;

internal static class AuthEndpoints
{
    internal static void MapAuthEndpoints(this RouteGroupBuilder root)
    {
        var auth = root.MapGroup("/auth")
            .AllowAnonymous()
            .WithTags("Auth");

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
        .WithSummary("Connexion (cookie)")
        .WithDescription("Permet à un utilisateur de se connecter en utilisant un cookie d'authentification.");
    }
}
