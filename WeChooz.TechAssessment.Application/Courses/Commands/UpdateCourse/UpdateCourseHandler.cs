using Shared.Mediator;
using WeChooz.TechAssessment.Application.Interfaces.Courses;

namespace WeChooz.TechAssessment.Application.Courses.Commands.UpdateCourse;

public sealed class UpdateCourseHandler(ICourseRepository courses) : IRequestHandler<UpdateCourseCommand, UpdateCourseResponse>
{
    public async Task<UpdateCourseResponse> HandleAsync(UpdateCourseCommand request, CancellationToken cancellationToken = default)
    {
        var updated = await courses.UpdateAsync(
            request.CourseId,
            request.Name,
            request.ShortDescription,
            request.LongDescriptionMarkdown,
            request.DurationDays,
            request.CseAudience,
            request.MaxCapacity,
            request.TrainerFirstName,
            request.TrainerLastName,
            cancellationToken);
        return new UpdateCourseResponse(updated);
    }
}
