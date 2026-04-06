using Microsoft.AspNetCore.Http.HttpResults;
using Shared.Mediator;
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
        var group = root.MapGroup("/public")
            .AllowAnonymous()
            .WithTags("Sessions - Public");

        group.MapGet("/sessions", async Task<Ok<List<GetPublicSessionsItem>>> (
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
        .WithSummary("Retourne la liste des sessions publiques disponibles")
        .WithDescription("Retourne la liste des sessions publiques disponibles, avec la possibilité de filtrer par audience, mode de livraison et dates de début");

        group.MapGet("/sessions/{sessionId:int}", async Task<Results<Ok<GetPublicSessionDetailResponse>, NotFound>> (
            IMediator mediator,
            int sessionId,
            CancellationToken cancellationToken) =>
        {
            var detail = await mediator.SendAsync(new GetPublicSessionDetailQuery(sessionId), cancellationToken);
            return detail is null ? TypedResults.NotFound() : TypedResults.Ok(detail);
        })
        .WithSummary("Retourne les détails d'une session publique")
        .WithDescription("Retourne les détails d'une session publique spécifique, identifiée par son ID");
    }

    private static void MapAdmin(RouteGroupBuilder root)
    {
        var group = root.MapGroup("/admin/sessions")
            .RequireAuthorization("Formation")
            .WithTags("Sessions - Admin");

        group.MapGet("/", async Task<Ok<List<GetAdminSessionsItem>>> (IMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.SendAsync(new GetAdminSessionsQuery(), cancellationToken);
            return TypedResults.Ok(result.Items.ToList());
        })
        .WithSummary("Retourne la liste de toutes les sessions")
        .WithSummary("Retourne la liste de toutes les sessions, sans filtrage, pour les administrateurs");

        group.MapGet("/{sessionId:int}", async Task<Results<Ok<GetAdminSessionByIdResponse>, NotFound>> (
            IMediator mediator,
            int sessionId,
            CancellationToken cancellationToken) =>
        {
            var session = await mediator.SendAsync(new GetAdminSessionByIdQuery(sessionId), cancellationToken);
            return session is null ? TypedResults.NotFound() : TypedResults.Ok(session);
        });

        group.MapPost("/", async Task<Created<CreateSessionResponse>> (IMediator mediator, CreateSessionCommand body, CancellationToken cancellationToken) =>
        {
            var result = await mediator.SendAsync(body, cancellationToken);
            return TypedResults.Created($"/api/admin/sessions/{result.SessionId}", result);
        })
        .WithSummary("Crée une nouvelle session")
        .WithDescription("Permet aux administrateurs de créer une nouvelle session en fournissant les détails nécessaires dans le corps de la requête");

        group.MapPut("/{sessionId:int}", async Task<Results<NoContent, NotFound>> (
            IMediator mediator,
            int sessionId,
            UpdateSessionCommand body,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.SendAsync(body with { SessionId = sessionId }, cancellationToken);
            return result.Updated ? TypedResults.NoContent() : TypedResults.NotFound();
        })
        .WithSummary("Met à jour une session existante")
        .WithDescription("Permet aux administrateurs de mettre à jour une session existante en fournissant les détails à mettre à jour dans le corps de la requête, et en spécifiant l'ID de la session dans l'URL");

        group.MapDelete("/{sessionId:int}", async Task<NoContent> (IMediator mediator, int sessionId, CancellationToken cancellationToken) =>
        {
            await mediator.SendAsync(new DeleteSessionCommand(sessionId), cancellationToken);
            return TypedResults.NoContent();
        })
        .WithSummary("Supprime une session")
        .WithSummary("Permet aux administrateurs de supprimer une session en spécifiant son ID dans l'URL");
    }
}
