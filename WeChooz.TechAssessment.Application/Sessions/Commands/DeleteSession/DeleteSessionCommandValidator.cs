using FluentValidation;

namespace WeChooz.TechAssessment.Application.Sessions.Commands.DeleteSession;

public sealed class DeleteSessionCommandValidator : AbstractValidator<DeleteSessionCommand>
{
    public DeleteSessionCommandValidator()
    {
        RuleFor(x => x.SessionId).GreaterThan(0);
    }
}
