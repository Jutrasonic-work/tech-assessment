using Microsoft.AspNetCore.Http.HttpResults;
using Shared.Mediator.Application;
using WeChooz.TechAssessment.Application.PublicSessions.Queries.GetPublicSessionDetail;
using WeChooz.TechAssessment.Application.PublicSessions.Queries.GetPublicSessions;
using WeChooz.TechAssessment.Application.Sessions.Commands.CreateSession;
using WeChooz.TechAssessment.Application.Sessions.Commands.DeleteSession;
using WeChooz.TechAssessment.Application.Sessions.Commands.UpdateSession;
using WeChooz.TechAssessment.Application.Sessions.Queries.GetAdminSessionById;
using WeChooz.TechAssessment.Application.Sessions.Queries.GetAdminSessions;
using WeChooz.TechAssessment.Domain.Courses;
using WeChooz.TechAssessment.Domain.Sessions;

namespace WeChooz.TechAssessment.Web.Api;

internal static class SessionEndpoints
{
    internal static void MapSessionEndpoints(this RouteGroupBuilder root)
    {
        MapPublic(root);
        MapAdmin(root);
    }

    private static void MapPublic(RouteGroupBuilder root)
    {
        var g = root.MapGroup("/public").AllowAnonymous();

        g.MapGet("/sessions", async Task<Ok<List<GetPublicSessionsItem>>> (
            IMediator mediator,
            CseAudience? audience,
            SessionDeliveryMode? deliveryMode,
            DateTime? startAfter,
            DateTime? startBefore,
            DateTime? startFrom,
            DateTime? startTo,
            CancellationToken cancellationToken) =>
        {
            var query = new GetPublicSessionsQuery(audience, deliveryMode, startAfter, startBefore, startFrom, startTo);
            var result = await mediator.SendAsync(query, cancellationToken);
            return TypedResults.Ok(result.Items.ToList());
        })
        .WithTags("Public — Sessions")
        .WithSummary("Lister les sessions (catalogue public)")
        .WithDescription("Filtres query optionnels : audience, deliveryMode, plages de dates (startAfter/startBefore ou startFrom/startTo).");

        g.MapGet("/sessions/{sessionId:int}", async Task<Results<Ok<GetPublicSessionDetailResponse>, NotFound>> (
            IMediator mediator,
            int sessionId,
            CancellationToken cancellationToken) =>
        {
            var detail = await mediator.SendAsync(new GetPublicSessionDetailQuery(sessionId), cancellationToken);
            return detail is null ? TypedResults.NotFound() : TypedResults.Ok(detail);
        })
        .WithTags("Public — Sessions")
        .WithSummary("Détail d'une session (public)")
        .WithDescription("La description longue est renvoyée en HTML (Markdown rendu côté serveur).");
    }

    private static void MapAdmin(RouteGroupBuilder root)
    {
        var g = root.MapGroup("/admin/sessions").RequireAuthorization("Formation");

        g.MapGet("/", async Task<Ok<List<GetAdminSessionsItem>>> (IMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.SendAsync(new GetAdminSessionsQuery(), cancellationToken);
            return TypedResults.Ok(result.Items.ToList());
        })
        .WithTags("Admin — Sessions")
        .WithSummary("Lister les sessions (admin)")
        .WithDescription("Authentification cookie, rôle formation.");

        g.MapGet("/{sessionId:int}", async Task<Results<Ok<GetAdminSessionByIdResponse>, NotFound>> (
            IMediator mediator,
            int sessionId,
            CancellationToken cancellationToken) =>
        {
            var session = await mediator.SendAsync(new GetAdminSessionByIdQuery(sessionId), cancellationToken);
            return session is null ? TypedResults.NotFound() : TypedResults.Ok(session);
        })
        .WithTags("Admin — Sessions")
        .WithSummary("Détail d'une session (admin)");

        g.MapPost("/", async Task<Created<CreateSessionResponse>> (IMediator mediator, CreateSessionCommand body, CancellationToken cancellationToken) =>
        {
            var result = await mediator.SendAsync(body, cancellationToken);
            return TypedResults.Created($"/api/admin/sessions/{result.SessionId}", result);
        })
        .WithTags("Admin — Sessions")
        .WithSummary("Créer une session");

        g.MapPut("/{sessionId:int}", async Task<Results<NoContent, NotFound>> (
            IMediator mediator,
            int sessionId,
            UpdateSessionCommand body,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.SendAsync(body with { SessionId = sessionId }, cancellationToken);
            return result.Updated ? TypedResults.NoContent() : TypedResults.NotFound();
        })
        .WithTags("Admin — Sessions")
        .WithSummary("Mettre à jour une session");

        g.MapDelete("/{sessionId:int}", async Task<NoContent> (IMediator mediator, int sessionId, CancellationToken cancellationToken) =>
        {
            await mediator.SendAsync(new DeleteSessionCommand(sessionId), cancellationToken);
            return TypedResults.NoContent();
        })
        .WithTags("Admin — Sessions")
        .WithSummary("Supprimer une session");
    }
}
