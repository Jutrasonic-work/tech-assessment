using WeChooz.TechAssessment.Domain.Sessions;

namespace WeChooz.TechAssessment.Domain.ReadModels;

public sealed record AdminSessionListItemResult(
    int SessionId,
    int CourseId,
    string CourseName,
    DateTime StartDate,
    SessionDeliveryMode DeliveryMode,
    int ParticipantCount,
    int MaxCapacity);

public sealed record AdminSessionDetailResult(
    int SessionId,
    int CourseId,
    string CourseName,
    DateTime StartDate,
    SessionDeliveryMode DeliveryMode,
    int ParticipantCount,
    int MaxCapacity);
