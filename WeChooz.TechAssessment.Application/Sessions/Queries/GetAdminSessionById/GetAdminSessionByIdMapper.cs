using WeChooz.TechAssessment.Domain.Sessions;

namespace WeChooz.TechAssessment.Application.Sessions.Queries.GetAdminSessionById;

internal static class GetAdminSessionByIdMapper
{
    public static GetAdminSessionByIdResponse ToResponse(this AdminSessionOverview s) =>
        new(s.SessionId, s.CourseId, s.CourseName, s.StartDate, s.DeliveryMode, s.ParticipantCount, s.MaxCapacity);
}
