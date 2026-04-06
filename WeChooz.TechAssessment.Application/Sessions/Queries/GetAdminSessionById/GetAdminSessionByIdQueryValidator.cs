using FluentValidation;

namespace WeChooz.TechAssessment.Application.Sessions.Queries.GetAdminSessionById;

public sealed class GetAdminSessionByIdQueryValidator : AbstractValidator<GetAdminSessionByIdQuery>
{
    public GetAdminSessionByIdQueryValidator()
    {
        RuleFor(x => x.SessionId).GreaterThan(0);
    }
}
