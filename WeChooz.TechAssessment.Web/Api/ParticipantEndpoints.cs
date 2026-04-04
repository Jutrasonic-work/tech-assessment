using Microsoft.AspNetCore.Http.HttpResults;
using Shared.Mediator.Application;
using WeChooz.TechAssessment.Application.Participants.Commands.AddParticipant;
using WeChooz.TechAssessment.Application.Participants.Commands.RemoveParticipant;
using WeChooz.TechAssessment.Application.Participants.Commands.UpdateParticipant;
using WeChooz.TechAssessment.Application.Participants.Queries.GetParticipantsBySession;

namespace WeChooz.TechAssessment.Web.Api;

internal static class ParticipantEndpoints
{
    internal static void MapParticipantEndpoints(this RouteGroupBuilder root)
    {
        var g = root.MapGroup("/admin/sessions/{sessionId:int}/participants").RequireAuthorization("Participants");

        g.MapGet("/", async Task<Ok<List<GetParticipantsBySessionItem>>> (
            IMediator mediator,
            int sessionId,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.SendAsync(new GetParticipantsBySessionQuery(sessionId), cancellationToken);
            return TypedResults.Ok(result.Items.ToList());
        })
        .WithTags("Admin — Participants")
        .WithSummary("Lister les participants d'une session")
        .WithDescription("Politique « Participants » : rôles formation ou sales.");

        g.MapPost("/", async Task<Created<AddParticipantResponse>> (
            IMediator mediator,
            int sessionId,
            AddParticipantCommand body,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.SendAsync(body with { SessionId = sessionId }, cancellationToken);
            return TypedResults.Created($"/api/admin/sessions/{sessionId}/participants", result);
        })
        .WithTags("Admin — Participants")
        .WithSummary("Ajouter un participant à une session");

        var byId = root.MapGroup("/admin/participants").RequireAuthorization("Participants");

        byId.MapPut("/{participantId:int}", async Task<Results<NoContent, NotFound>> (
            IMediator mediator,
            int participantId,
            UpdateParticipantCommand body,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.SendAsync(body with { ParticipantId = participantId }, cancellationToken);
            return result.Updated ? TypedResults.NoContent() : TypedResults.NotFound();
        })
        .WithTags("Admin — Participants")
        .WithSummary("Mettre à jour un participant");

        byId.MapDelete("/{participantId:int}", async Task<NoContent> (IMediator mediator, int participantId, CancellationToken cancellationToken) =>
        {
            await mediator.SendAsync(new RemoveParticipantCommand(participantId), cancellationToken);
            return TypedResults.NoContent();
        })
        .WithTags("Admin — Participants")
        .WithSummary("Supprimer un participant");
    }
}
