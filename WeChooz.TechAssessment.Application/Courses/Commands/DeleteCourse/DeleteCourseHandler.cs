using Shared.Mediator.Application;
using WeChooz.TechAssessment.Application.Persistence.Courses;

namespace WeChooz.TechAssessment.Application.Courses.Commands.DeleteCourse;

public sealed class DeleteCourseHandler(ICourseRepository courses) : IRequestHandler<DeleteCourseCommand>
{
    public Task HandleAsync(DeleteCourseCommand request, CancellationToken cancellationToken = default) =>
        courses.DeleteAsync(request.CourseId, cancellationToken);
}
