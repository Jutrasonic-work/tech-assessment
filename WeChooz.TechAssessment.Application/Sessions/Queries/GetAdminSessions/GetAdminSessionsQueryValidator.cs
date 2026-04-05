using FluentValidation;

namespace WeChooz.TechAssessment.Application.Sessions.Queries.GetAdminSessions;

public sealed class GetAdminSessionsQueryValidator : AbstractValidator<GetAdminSessionsQuery>
{
    public GetAdminSessionsQueryValidator()
    {
        // Requête sans paramètres.
    }
}
