using WeChooz.TechAssessment.Domain.Sessions;

namespace WeChooz.TechAssessment.Application.Sessions.Queries.GetAdminSessions;

public sealed record GetAdminSessionsResponse(IReadOnlyList<GetAdminSessionsItem> Items);

public sealed record GetAdminSessionsItem(
    int SessionId,
    int CourseId,
    string CourseName,
    DateTime StartDate,
    SessionDeliveryMode DeliveryMode,
    int ParticipantCount,
    int MaxCapacity);
