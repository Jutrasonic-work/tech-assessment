using WeChooz.TechAssessment.Domain.Courses;
using WeChooz.TechAssessment.Domain.Sessions;

namespace WeChooz.TechAssessment.Application.PublicSessions.Queries.GetPublicSessionDetail;

public sealed record GetPublicSessionDetailResponse(
    int SessionId,
    string CourseName,
    string ShortDescription,
    string LongDescriptionHtml,
    CseAudience CseAudience,
    DateTime StartDate,
    int DurationDays,
    SessionDeliveryMode DeliveryMode,
    int RemainingSeats,
    string TrainerFirstName,
    string TrainerLastName);
