using FluentValidation;

namespace WeChooz.TechAssessment.Application.Courses.Commands.DeleteCourse;

public sealed class DeleteCourseCommandValidator : AbstractValidator<DeleteCourseCommand>
{
    public DeleteCourseCommandValidator()
    {
        RuleFor(x => x.CourseId).GreaterThan(0);
    }
}
