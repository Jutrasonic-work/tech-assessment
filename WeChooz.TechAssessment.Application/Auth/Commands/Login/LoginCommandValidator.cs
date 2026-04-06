using FluentValidation;

namespace WeChooz.TechAssessment.Application.Auth.Commands.Login;

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Login)
            .NotNull()
            .Must(s => !string.IsNullOrWhiteSpace(s));
    }
}
