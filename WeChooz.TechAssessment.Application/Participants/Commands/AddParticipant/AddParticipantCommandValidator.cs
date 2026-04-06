using FluentValidation;

namespace WeChooz.TechAssessment.Application.Participants.Commands.AddParticipant;

public sealed class AddParticipantCommandValidator : AbstractValidator<AddParticipantCommand>
{
    public AddParticipantCommandValidator()
    {
        RuleFor(x => x.SessionId).GreaterThan(0);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(320);
        RuleFor(x => x.CompanyName).NotEmpty().MaximumLength(500);
    }
}
