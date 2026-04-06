using FluentValidation;

namespace WeChooz.TechAssessment.Application.PublicSessions.Queries.GetPublicSessions;

public sealed class GetPublicSessionsQueryValidator : AbstractValidator<GetPublicSessionsQuery>
{
    public GetPublicSessionsQueryValidator()
    {
        RuleFor(x => x)
            .Must(q => !q.StartAfter.HasValue || !q.StartBefore.HasValue || q.StartAfter <= q.StartBefore)
            .WithMessage("StartAfter doit être antérieur ou égal à StartBefore.");

        RuleFor(x => x)
            .Must(q => !q.StartFrom.HasValue || !q.StartTo.HasValue || q.StartFrom <= q.StartTo)
            .WithMessage("StartFrom doit être antérieur ou égal à StartTo.");
    }
}
