using Shared.Mediator;

namespace WeChooz.TechAssessment.Application.Courses.Queries.GetCourseById;

public sealed record GetCourseByIdQuery(int CourseId) : IRequest<GetCourseByIdResponse?>;
