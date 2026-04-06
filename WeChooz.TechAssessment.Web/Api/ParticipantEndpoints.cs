using Microsoft.AspNetCore.Http.HttpResults;
using Shared.Mediator;
using WeChooz.TechAssessment.Application.Participants.Commands.AddParticipant;
using WeChooz.TechAssessment.Application.Participants.Commands.RemoveParticipant;
using WeChooz.TechAssessment.Application.Participants.Commands.UpdateParticipant;
using WeChooz.TechAssessment.Application.Participants.Queries.GetParticipantsBySession;

namespace WeChooz.TechAssessment.Web.Api;

internal static class ParticipantEndpoints
{
    internal static void MapParticipantEndpoints(this RouteGroupBuilder root)
    {
        var group = root.MapGroup("/admin/sessions/{sessionId:int}/participants")
            .RequireAuthorization("Participants")
            .WithTags("Participants");

        group.MapGet("/", async Task<Ok<List<GetParticipantsBySessionItem>>> (
            IMediator mediator,
            int sessionId,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.SendAsync(new GetParticipantsBySessionQuery(sessionId), cancellationToken);
            return TypedResults.Ok(result.Items.ToList());
        })
        .WithSummary("Retourne la liste des participants d'une session")
        .WithDescription("Liste des participants d'une session donnée. Elle est accessible uniquement aux utilisateurs ayant le rôle 'Participants'.");

        group.MapPost("/", async Task<Created<AddParticipantResponse>> (
            IMediator mediator,
            int sessionId,
            AddParticipantCommand body,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.SendAsync(body with { SessionId = sessionId }, cancellationToken);
            return TypedResults.Created($"/api/admin/sessions/{sessionId}/participants", result);
        })
        .WithSummary("Ajoute un participant à une session")
        .WithDescription("Ajoute un participant à une session donnée. Elle est accessible uniquement aux utilisateurs ayant le rôle 'Participants'.");

        var groupById = root.MapGroup("/admin/participants")
            .RequireAuthorization("Participants")
            .WithTags("Participants");

        groupById.MapPut("/{participantId:int}", async Task<Results<NoContent, NotFound>> (
            IMediator mediator,
            int participantId,
            UpdateParticipantCommand body,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.SendAsync(body with { ParticipantId = participantId }, cancellationToken);
            return result.Updated ? TypedResults.NoContent() : TypedResults.NotFound();
        })
        .WithSummary("Met à jour les informations d'un participant")
        .WithDescription("Met à jour les informations d'un participant donné. Elle est accessible uniquement aux utilisateurs ayant le rôle 'Participants'.");

        groupById.MapDelete("/{participantId:int}", async Task<NoContent> (IMediator mediator, int participantId, CancellationToken cancellationToken) =>
        {
            await mediator.SendAsync(new RemoveParticipantCommand(participantId), cancellationToken);
            return TypedResults.NoContent();
        })
        .WithSummary("Supprime un participant")
        .WithDescription("Supprime un participant donné. Elle est accessible uniquement aux utilisateurs ayant le rôle 'Participants'.");
    }
}
