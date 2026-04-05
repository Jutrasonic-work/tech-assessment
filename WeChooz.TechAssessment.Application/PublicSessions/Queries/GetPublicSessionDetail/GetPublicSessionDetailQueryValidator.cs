using FluentValidation;

namespace WeChooz.TechAssessment.Application.PublicSessions.Queries.GetPublicSessionDetail;

public sealed class GetPublicSessionDetailQueryValidator : AbstractValidator<GetPublicSessionDetailQuery>
{
    public GetPublicSessionDetailQueryValidator()
    {
        RuleFor(x => x.SessionId).GreaterThan(0);
    }
}
