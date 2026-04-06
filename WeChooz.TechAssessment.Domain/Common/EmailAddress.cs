using System.Text.RegularExpressions;

namespace WeChooz.TechAssessment.Domain.Common;

public sealed partial record EmailAddress
{
    public string Value { get; }

    public EmailAddress(string value)
    {
        Value = Normalize(value);
    }

    private static string Normalize(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("L'e-mail est requis.", nameof(value));
        }

        var trimmed = value.Trim();
        if (trimmed.Length > 320)
        {
            throw new ArgumentException("L'e-mail dépasse 320 caractères.", nameof(value));
        }

        if (!SimpleEmailRegex().IsMatch(trimmed))
        {
            throw new ArgumentException("Le format de l'e-mail est invalide.", nameof(value));
        }

        return trimmed;
    }

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.CultureInvariant | RegexOptions.Compiled)]
    private static partial Regex SimpleEmailRegex();
}
