using Shared.Mediator.Application;
using WeChooz.TechAssessment.Application.Persistence.Courses;

namespace WeChooz.TechAssessment.Application.Courses.Queries.GetCourseById;

public sealed class GetCourseByIdHandler(ICourseRepository courses) : IRequestHandler<GetCourseByIdQuery, GetCourseByIdResponse?>
{
    public async Task<GetCourseByIdResponse?> HandleAsync(GetCourseByIdQuery request, CancellationToken cancellationToken = default)
    {
        var row = await courses.GetByIdAsync(request.CourseId, cancellationToken);
        return row?.ToResponse();
    }
}
