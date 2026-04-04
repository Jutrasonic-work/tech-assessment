using WeChooz.TechAssessment.Domain.Courses;
using WeChooz.TechAssessment.Domain.Sessions;

namespace WeChooz.TechAssessment.Infrastructure.Data.Repositories.Sessions.ReadModels;

internal sealed record PublicSessionDetailRow(
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
