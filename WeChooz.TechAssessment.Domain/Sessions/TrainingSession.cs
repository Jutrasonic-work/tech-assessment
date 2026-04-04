namespace WeChooz.TechAssessment.Domain.Sessions;

/// <summary>
/// Session de formation planifiée : un cours donné à une date, avec un mode de délivrance.
/// </summary>
public sealed class TrainingSession
{
    public int SessionId { get; private set; }
    public int CourseId { get; private set; }
    public DateTime StartDate { get; private set; }
    public SessionDeliveryMode DeliveryMode { get; private set; }

    public TrainingSession(int sessionId, int courseId, DateTime startDate, SessionDeliveryMode deliveryMode)
    {
        if (sessionId < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(sessionId));
        }

        if (courseId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(courseId));
        }

        SessionId = sessionId;
        CourseId = courseId;
        StartDate = startDate;
        DeliveryMode = deliveryMode;
    }

    public void Reschedule(DateTime startDate, SessionDeliveryMode deliveryMode)
    {
        StartDate = startDate;
        DeliveryMode = deliveryMode;
    }

    public void ReassignCourse(int courseId)
    {
        if (courseId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(courseId));
        }

        CourseId = courseId;
    }

    public void AssignIdentity(int sessionId)
    {
        if (SessionId != 0)
        {
            throw new InvalidOperationException("L’identifiant de la session est déjà défini.");
        }

        if (sessionId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(sessionId));
        }

        SessionId = sessionId;
    }
}
