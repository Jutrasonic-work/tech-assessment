using Shared.Mediator.Application;
using WeChooz.TechAssessment.Domain.Repositories;

namespace WeChooz.TechAssessment.Application.Courses.Commands.CreateCourse;

public sealed class CreateCourseHandler(ICourseRepository courses) : IRequestHandler<CreateCourseCommand, CreateCourseResponse>
{
    public async Task<CreateCourseResponse> HandleAsync(CreateCourseCommand request, CancellationToken cancellationToken = default)
    {
        var id = await courses.InsertAsync(
            request.Name,
            request.ShortDescription,
            request.LongDescriptionMarkdown,
            request.DurationDays,
            request.CseAudience,
            request.MaxCapacity,
            request.TrainerFirstName,
            request.TrainerLastName,
            cancellationToken);
        return new CreateCourseResponse(id);
    }
}
