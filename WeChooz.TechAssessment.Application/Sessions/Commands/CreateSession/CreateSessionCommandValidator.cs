using FluentValidation;

namespace WeChooz.TechAssessment.Application.Sessions.Commands.CreateSession;

public sealed class CreateSessionCommandValidator : AbstractValidator<CreateSessionCommand>
{
    public CreateSessionCommandValidator()
    {
        RuleFor(x => x.CourseId).GreaterThan(0);
        RuleFor(x => x.DeliveryMode).IsInEnum();
    }
}
