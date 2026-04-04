using WeChooz.TechAssessment.Domain.ReadModels;

namespace WeChooz.TechAssessment.Application.Sessions.Queries.GetAdminSessionById;

internal static class GetAdminSessionByIdMapper
{
    public static GetAdminSessionByIdResponse ToResponse(this AdminSessionDetailResult r) =>
        new(r.SessionId, r.CourseId, r.CourseName, r.StartDate, r.DeliveryMode, r.ParticipantCount, r.MaxCapacity);
}
