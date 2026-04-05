using FluentValidation;

namespace WeChooz.TechAssessment.Application.Sessions.Commands.UpdateSession;

public sealed class UpdateSessionCommandValidator : AbstractValidator<UpdateSessionCommand>
{
    public UpdateSessionCommandValidator()
    {
        RuleFor(x => x.SessionId).GreaterThan(0);
        RuleFor(x => x.CourseId).GreaterThan(0);
        RuleFor(x => x.DeliveryMode).IsInEnum();
    }
}
