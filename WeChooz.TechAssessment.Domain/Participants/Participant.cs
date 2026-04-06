using WeChooz.TechAssessment.Domain.Common;

namespace WeChooz.TechAssessment.Domain.Participants;

public sealed class Participant
{
    public int ParticipantId { get; private set; }
    public int SessionId { get; private set; }
    public PersonName Name { get; private set; }
    public EmailAddress Email { get; private set; }
    public string CompanyName { get; private set; }

    public Participant(int participantId, int sessionId, PersonName name, EmailAddress email, string companyName)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(participantId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(sessionId);

        ParticipantId = participantId;
        SessionId = sessionId;
        Name = name;
        Email = email;
        CompanyName = RequireNonWhiteSpace(companyName, nameof(companyName));
    }

    public void UpdateDetails(PersonName name, EmailAddress email, string companyName)
    {
        Name = name;
        Email = email;
        CompanyName = RequireNonWhiteSpace(companyName, nameof(companyName));
    }

    public void AssignIdentity(int participantId)
    {
        if (ParticipantId != 0)
        {
            throw new InvalidOperationException("L'identifiant du participant est déjà défini.");
        }

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(participantId);

        ParticipantId = participantId;
    }

    private static string RequireNonWhiteSpace(string value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("La valeur ne peut pas être vide.", paramName);
        }

        return value.Trim();
    }
}
