using WeChooz.TechAssessment.Domain.Courses;
using WeChooz.TechAssessment.Domain.Sessions;

namespace WeChooz.TechAssessment.Domain.ReadModels;

public sealed record PublicSessionListItemResult(
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

public sealed record PublicSessionDetailResult(
    int SessionId,
    string CourseName,
    string ShortDescription,
    string LongDescriptionMarkdown,
    CseAudience CseAudience,
    DateTime StartDate,
    int DurationDays,
    SessionDeliveryMode DeliveryMode,
    int RemainingSeats,
    string TrainerFirstName,
    string TrainerLastName);

public sealed record ListPublicSessionsFilter(
    CseAudience? Audience,
    SessionDeliveryMode? DeliveryMode,
    DateTime? StartAfter,
    DateTime? StartBefore,
    DateTime? StartFrom,
    DateTime? StartTo);
