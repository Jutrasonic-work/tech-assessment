using Shared.Mediator.Application;

namespace WeChooz.TechAssessment.Application.Courses.Queries.GetCourseById;

public sealed record GetCourseByIdQuery(int CourseId) : IRequest<GetCourseByIdResponse?>;
