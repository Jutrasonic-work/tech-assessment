namespace WeChooz.TechAssessment.Domain.Common;

public sealed record PersonName
{
    public string FirstName { get; }
    public string LastName { get; }

    public PersonName(string firstName, string lastName)
    {
        FirstName = RequireNonWhiteSpace(firstName, nameof(firstName));
        LastName = RequireNonWhiteSpace(lastName, nameof(lastName));
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
