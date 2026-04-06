using FluentValidation;

namespace WeChooz.TechAssessment.Application.Courses.Commands.UpdateCourse;

public sealed class UpdateCourseCommandValidator : AbstractValidator<UpdateCourseCommand>
{
    public UpdateCourseCommandValidator()
    {
        RuleFor(x => x.CourseId).GreaterThan(0);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(500);
        RuleFor(x => x.ShortDescription).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.LongDescriptionMarkdown).NotEmpty();
        RuleFor(x => x.DurationDays).GreaterThan(0);
        RuleFor(x => x.CseAudience).IsInEnum();
        RuleFor(x => x.MaxCapacity).GreaterThan(0);
        RuleFor(x => x.TrainerFirstName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.TrainerLastName).NotEmpty().MaximumLength(200);
    }
}
