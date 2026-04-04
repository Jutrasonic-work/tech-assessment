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
        });

        g.MapGet("/{courseId:int}", async Task<Results<Ok<GetCourseByIdResponse>, NotFound>> (
            IMediator mediator,
            int courseId,
            CancellationToken cancellationToken) =>
        {
            var course = await mediator.SendAsync(new GetCourseByIdQuery(courseId), cancellationToken);
            return course is null ? TypedResults.NotFound() : TypedResults.Ok(course);
        });

        g.MapPost("/", async Task<Created<CreateCourseResponse>> (IMediator mediator, CreateCourseCommand body, CancellationToken cancellationToken) =>
        {
            var result = await mediator.SendAsync(body, cancellationToken);
            return TypedResults.Created($"/api/admin/courses/{result.CourseId}", result);
        });

        g.MapPut("/{courseId:int}", async Task<Results<NoContent, NotFound>> (
            IMediator mediator,
            int courseId,
            UpdateCourseCommand body,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.SendAsync(body with { CourseId = courseId }, cancellationToken);
            return result.Updated ? TypedResults.NoContent() : TypedResults.NotFound();
        });

        g.MapDelete("/{courseId:int}", async Task<NoContent> (IMediator mediator, int courseId, CancellationToken cancellationToken) =>
        {
            await mediator.SendAsync(new DeleteCourseCommand(courseId), cancellationToken);
            return TypedResults.NoContent();
        });
    }
}
