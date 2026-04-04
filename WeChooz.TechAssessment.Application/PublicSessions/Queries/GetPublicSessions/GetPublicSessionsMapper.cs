using WeChooz.TechAssessment.Domain.ReadModels;

namespace WeChooz.TechAssessment.Application.PublicSessions.Queries.GetPublicSessions;

internal static class GetPublicSessionsMapper
{
    public static GetPublicSessionsItem ToItem(this PublicSessionListItemResult r) =>
        new(
            r.SessionId,
            r.CourseName,
            r.ShortDescription,
            r.CseAudience,
            r.StartDate,
            r.DurationDays,
            r.DeliveryMode,
            r.RemainingSeats,
            r.TrainerFirstName,
            r.TrainerLastName);
}
