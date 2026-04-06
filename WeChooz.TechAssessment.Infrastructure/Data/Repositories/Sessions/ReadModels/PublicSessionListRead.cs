using WeChooz.TechAssessment.Domain.Courses;
using WeChooz.TechAssessment.Domain.Sessions;

namespace WeChooz.TechAssessment.Infrastructure.Data.Repositories.Sessions.ReadModels;

internal sealed record PublicSessionListRead(
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
