using WeChooz.TechAssessment.Domain.Common;

namespace WeChooz.TechAssessment.Domain.Courses;

public sealed class Course
{
    public int CourseId { get; private set; }
    public string Name { get; private set; }
    public string ShortDescription { get; private set; }
    public string LongDescription { get; private set; }
    public int DurationDays { get; private set; }
    public CseAudience CseAudience { get; private set; }
    public int MaxCapacity { get; private set; }
    public PersonName Trainer { get; private set; }

    public Course(
        int courseId,
        string name,
        string shortDescription,
        string longDescription,
        int durationDays,
        CseAudience cseAudience,
        int maxCapacity,
        PersonName trainer)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(courseId, 0);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(durationDays);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxCapacity);

        CourseId = courseId;
        Name = RequireNonWhiteSpace(name, nameof(name));
        ShortDescription = RequireNonWhiteSpace(shortDescription, nameof(shortDescription));
        LongDescription = RequireNonWhiteSpace(longDescription, nameof(longDescription));
        DurationDays = durationDays;
        CseAudience = cseAudience;
        MaxCapacity = maxCapacity;
        Trainer = trainer;
    }

    public void UpdateDetails(
        string name,
        string shortDescription,
        string longDescription,
        int durationDays,
        CseAudience cseAudience,
        int maxCapacity,
        PersonName trainer)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(durationDays);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxCapacity);

        Name = RequireNonWhiteSpace(name, nameof(name));
        ShortDescription = RequireNonWhiteSpace(shortDescription, nameof(shortDescription));
        LongDescription = RequireNonWhiteSpace(longDescription, nameof(longDescription));
        DurationDays = durationDays;
        CseAudience = cseAudience;
        MaxCapacity = maxCapacity;
        Trainer = trainer;
    }

    public void AssignIdentity(int courseId)
    {
        if (CourseId != 0)
        {
            throw new InvalidOperationException("L'identifiant du cours est déjà défini.");
        }

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(courseId);

        CourseId = courseId;
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
