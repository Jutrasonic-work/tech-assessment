namespace WeChooz.TechAssessment.Domain.Sessions;

/// <summary>Vue session pour l'administration (effectifs + cours).</summary>
public sealed record AdminSessionOverview(
    int SessionId,
    int CourseId,
    string CourseName,
    DateTime StartDate,
    SessionDeliveryMode DeliveryMode,
    int ParticipantCount,
    int MaxCapacity);
