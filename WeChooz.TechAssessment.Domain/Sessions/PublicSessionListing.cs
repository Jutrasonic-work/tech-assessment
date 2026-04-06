using WeChooz.TechAssessment.Domain.Courses;

namespace WeChooz.TechAssessment.Domain.Sessions;

public sealed record PublicSessionListing(
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
