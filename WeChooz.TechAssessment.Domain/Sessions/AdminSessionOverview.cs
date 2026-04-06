namespace WeChooz.TechAssessment.Domain.Sessions;

public sealed record AdminSessionOverview(
    int SessionId,
    int CourseId,
    string CourseName,
    DateTime StartDate,
    SessionDeliveryMode DeliveryMode,
    int ParticipantCount,
    int MaxCapacity);
