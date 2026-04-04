using Microsoft.AspNetCore.Http.HttpResults;
using Shared.Mediator.Application;
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
        var g = root.MapGroup("/admin/courses").RequireAuthorization("Formation");

        g.MapGet("/", async Task<Ok<List<GetCoursesItem>>> (IMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.SendAsync(new GetCoursesQuery(), cancellationToken);
            return TypedResults.Ok(result.Items.ToList());
        })
        .WithTags("Admin — Formations")
        .WithSummary("Lister les formations")
        .WithDescription("Retourne la liste des formations. Authentification cookie, rôle formation.");

        g.MapGet("/{courseId:int}", async Task<Results<Ok<GetCourseByIdResponse>, NotFound>> (
            IMediator mediator,
            int courseId,
            CancellationToken cancellationToken) =>
        {
            var course = await mediator.SendAsync(new GetCourseByIdQuery(courseId), cancellationToken);
            return course is null ? TypedResults.NotFound() : TypedResults.Ok(course);
        })
        .WithTags("Admin — Formations")
        .WithSummary("Détail d'une formation")
        .WithDescription("Retourne 404 si l'identifiant est inconnu.");

        g.MapPost("/", async Task<Created<CreateCourseResponse>> (IMediator mediator, CreateCourseCommand body, CancellationToken cancellationToken) =>
        {
            var result = await mediator.SendAsync(body, cancellationToken);
            return TypedResults.Created($"/api/admin/courses/{result.CourseId}", result);
        })
        .WithTags("Admin — Formations")
        .WithSummary("Créer une formation");

        g.MapPut("/{courseId:int}", async Task<Results<NoContent, NotFound>> (
            IMediator mediator,
            int courseId,
            UpdateCourseCommand body,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.SendAsync(body with { CourseId = courseId }, cancellationToken);
            return result.Updated ? TypedResults.NoContent() : TypedResults.NotFound();
        })
        .WithTags("Admin — Formations")
        .WithSummary("Mettre à jour une formation")
        .WithDescription("Le corps ne contient pas l'id : il est pris depuis l'URL.");

        g.MapDelete("/{courseId:int}", async Task<NoContent> (IMediator mediator, int courseId, CancellationToken cancellationToken) =>
        {
            await mediator.SendAsync(new DeleteCourseCommand(courseId), cancellationToken);
            return TypedResults.NoContent();
        })
        .WithTags("Admin — Formations")
        .WithSummary("Supprimer une formation");
    }
}
