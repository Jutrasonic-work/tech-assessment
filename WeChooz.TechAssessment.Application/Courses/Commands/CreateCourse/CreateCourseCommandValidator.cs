using FluentValidation;

namespace WeChooz.TechAssessment.Application.Courses.Commands.CreateCourse;

public sealed class CreateCourseCommandValidator : AbstractValidator<CreateCourseCommand>
{
    public CreateCourseCommandValidator()
    {
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
