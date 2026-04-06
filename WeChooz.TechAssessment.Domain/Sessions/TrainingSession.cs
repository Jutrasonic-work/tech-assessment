namespace WeChooz.TechAssessment.Domain.Sessions;

public sealed class TrainingSession
{
    public int SessionId { get; private set; }
    public int CourseId { get; private set; }
    public DateTime StartDate { get; private set; }
    public SessionDeliveryMode DeliveryMode { get; private set; }

    public TrainingSession(int sessionId, int courseId, DateTime startDate, SessionDeliveryMode deliveryMode)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(sessionId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(courseId);

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
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(courseId);

        CourseId = courseId;
    }

    public void AssignIdentity(int sessionId)
    {
        if (SessionId != 0)
        {
            throw new InvalidOperationException("L'identifiant de la session est déjà défini.");
        }

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(sessionId);

        SessionId = sessionId;
    }
}
