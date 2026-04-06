using WeChooz.TechAssessment.Domain.Sessions;

namespace WeChooz.TechAssessment.Application.PublicSessions.Queries.GetPublicSessions;

internal static class GetPublicSessionsMapper
{
    public static GetPublicSessionsItem ToItem(this PublicSessionListing s) =>
        new(
            s.SessionId,
            s.CourseName,
            s.ShortDescription,
            s.CseAudience,
            s.StartDate,
            s.DurationDays,
            s.DeliveryMode,
            s.RemainingSeats,
            s.TrainerFirstName,
            s.TrainerLastName);
}
