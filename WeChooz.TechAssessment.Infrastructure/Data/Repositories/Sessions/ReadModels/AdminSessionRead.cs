using WeChooz.TechAssessment.Domain.Sessions;

namespace WeChooz.TechAssessment.Infrastructure.Data.Repositories.Sessions.ReadModels;

internal sealed record AdminSessionRead(
    int SessionId,
    int CourseId,
    string CourseName,
    DateTime StartDate,
    SessionDeliveryMode DeliveryMode,
    int ParticipantCount,
    int MaxCapacity);
