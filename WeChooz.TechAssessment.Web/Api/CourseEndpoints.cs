using Microsoft.AspNetCore.Http.HttpResults;
using Shared.Mediator;
using WeChooz.TechAssessment.Application.Courses.Commands.CreateCourse;
using WeChooz.TechAssessment.Application.Courses.Commands.DeleteCourse;
using WeChooz.TechAssessment.Application.Courses.Commands.UpdateCourse;
using WeChooz.TechAssessment.Application.Courses.Queries.GetCourseById;
using WeChooz.TechAssessment.Application.Courses.Queries.GetCourses;

namespace WeChooz.TechAssessment.Web.Api;

internal static class CourseEndpoints
{
    internal static void MapCourseEndpoints(this RouteGroupBuilder root)
    {
        var group = root.MapGroup("/admin/courses")
            .RequireAuthorization("Formation")
            .WithTags("Formations");

        group.MapGet("/", async Task<Ok<List<GetCoursesItem>>> (IMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.SendAsync(new GetCoursesQuery(), cancellationToken);
            return TypedResults.Ok(result.Items.ToList());
        })
        .WithSummary("Retourne la liste de toutes les formations.")
        .WithDescription("Retourne la liste de toutes les formations disponibles.");

        group.MapGet("/{courseId:int}", async Task<Results<Ok<GetCourseByIdResponse>, NotFound>> (
            IMediator mediator,
            int courseId,
            CancellationToken cancellationToken) =>
        {
            var course = await mediator.SendAsync(new GetCourseByIdQuery(courseId), cancellationToken);
            return course is null ? TypedResults.NotFound() : TypedResults.Ok(course);
        })
        .WithSummary("Retourne les détails d'une formation.")
        .WithDescription("Retourne les détails d'une formation spécifique identifiée par son ID.");

        group.MapPost("/", async Task<Created<CreateCourseResponse>> (IMediator mediator, CreateCourseCommand body, CancellationToken cancellationToken) =>
        {
            var result = await mediator.SendAsync(body, cancellationToken);
            return TypedResults.Created($"/api/admin/courses/{result.CourseId}", result);
        })
        .WithSummary("Crée une nouvelle formation.")
        .WithDescription("Permet de créer une nouvelle formation en fournissant les détails nécessaires dans le corps de la requête.");

        group.MapPut("/{courseId:int}", async Task<Results<NoContent, NotFound>> (
            IMediator mediator,
            int courseId,
            UpdateCourseCommand body,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.SendAsync(body with { CourseId = courseId }, cancellationToken);
            return result.Updated ? TypedResults.NoContent() : TypedResults.NotFound();
        })
        .WithSummary("Met à jour une formation existante.")
        .WithDescription("Permet de mettre à jour les détails d'une formation existante identifiée par son ID en fournissant les nouvelles informations dans le corps de la requête.");

        group.MapDelete("/{courseId:int}", async Task<NoContent> (IMediator mediator, int courseId, CancellationToken cancellationToken) =>
        {
            await mediator.SendAsync(new DeleteCourseCommand(courseId), cancellationToken);
            return TypedResults.NoContent();
        })
        .WithSummary("Supprime une formation.")
        .WithDescription("Permet de supprimer une formation existante identifiée par son ID.");
    }
}
