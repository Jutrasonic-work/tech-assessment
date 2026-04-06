using WeChooz.TechAssessment.Domain.Courses;

namespace WeChooz.TechAssessment.Domain.Sessions;

public sealed record PublicSessionListCriteria(
    CseAudience? Audience,
    SessionDeliveryMode? DeliveryMode,
    DateTime? StartAfter,
    DateTime? StartBefore,
    DateTime? StartFrom,
    DateTime? StartTo);
