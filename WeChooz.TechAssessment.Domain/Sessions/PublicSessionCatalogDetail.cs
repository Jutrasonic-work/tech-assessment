using WeChooz.TechAssessment.Domain.Courses;

namespace WeChooz.TechAssessment.Domain.Sessions;

/// <summary>Fiche détaillée catalogue public (description longue en Markdown côté domaine).</summary>
public sealed record PublicSessionCatalogDetail(
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
