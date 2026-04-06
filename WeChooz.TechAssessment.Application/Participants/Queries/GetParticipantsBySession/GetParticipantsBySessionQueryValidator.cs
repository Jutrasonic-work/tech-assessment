using FluentValidation;

namespace WeChooz.TechAssessment.Application.Participants.Queries.GetParticipantsBySession;

public sealed class GetParticipantsBySessionQueryValidator : AbstractValidator<GetParticipantsBySessionQuery>
{
    public GetParticipantsBySessionQueryValidator()
    {
        RuleFor(x => x.SessionId).GreaterThan(0);
    }
}
