using WeChooz.TechAssessment.Domain.ReadModels;

namespace WeChooz.TechAssessment.Application.Sessions.Queries.GetAdminSessions;

internal static class GetAdminSessionsMapper
{
    public static GetAdminSessionsItem ToItem(this AdminSessionListItemResult r) =>
        new(r.SessionId, r.CourseId, r.CourseName, r.StartDate, r.DeliveryMode, r.ParticipantCount, r.MaxCapacity);
}
