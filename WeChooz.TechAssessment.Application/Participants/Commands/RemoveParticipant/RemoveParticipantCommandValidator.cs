using FluentValidation;

namespace WeChooz.TechAssessment.Application.Participants.Commands.RemoveParticipant;

public sealed class RemoveParticipantCommandValidator : AbstractValidator<RemoveParticipantCommand>
{
    public RemoveParticipantCommandValidator()
    {
        RuleFor(x => x.ParticipantId).GreaterThan(0);
    }
}
