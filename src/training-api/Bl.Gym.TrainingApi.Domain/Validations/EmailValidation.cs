using System.Diagnostics.CodeAnalysis;

namespace Bl.Gym.TrainingApi.Domain.Validations;

public class EmailValidation
{
    public const int MAX_EMAIL_SIZE = 100;
    public const int MIN_EMAIL_SIZE = 3;

    public const string REGEX_VALIDATION_EMAIL = @"^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";

    public static bool IsInvalidEmail([NotNullWhen(false)] string? email)
        => !IsValidEmail(email);

    public static bool IsValidEmail([NotNullWhen(true)] string? email)
    {
        if (email is null)
            return false;

        if (email.Length > MAX_EMAIL_SIZE ||
            email.Length < MIN_EMAIL_SIZE)
            return false;

        return System.Text.RegularExpressions.Regex.IsMatch(email, REGEX_VALIDATION_EMAIL, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
    }
}
