using Shared.Mediator;
using WeChooz.TechAssessment.Application.Interfaces.Courses;

namespace WeChooz.TechAssessment.Application.Courses.Queries.GetCourses;

public sealed class GetCoursesHandler(ICourseRepository courses) : IRequestHandler<GetCoursesQuery, GetCoursesResponse>
{
    public async Task<GetCoursesResponse> HandleAsync(GetCoursesQuery request, CancellationToken cancellationToken = default)
    {
        var rows = await courses.ListAsync(cancellationToken);
        var items = rows.Select(r => r.ToItem()).ToList();
        return new GetCoursesResponse(items);
    }
}
