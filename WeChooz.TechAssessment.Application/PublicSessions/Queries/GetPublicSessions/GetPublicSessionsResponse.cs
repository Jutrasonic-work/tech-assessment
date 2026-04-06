using WeChooz.TechAssessment.Domain.Courses;
using WeChooz.TechAssessment.Domain.Sessions;

namespace WeChooz.TechAssessment.Application.PublicSessions.Queries.GetPublicSessions;

public sealed record GetPublicSessionsResponse(IReadOnlyList<GetPublicSessionsItem> Items);

public sealed record GetPublicSessionsItem(
    int SessionId,
    string CourseName,
    string ShortDescription,
    CseAudience CseAudience,
    DateTime StartDate,
    int DurationDays,
    SessionDeliveryMode DeliveryMode,
    int RemainingSeats,
    string TrainerFirstName,
    string TrainerLastName);
