namespace Bl.Gym.TrainingApi.Domain.Validations;

public static class StringValidation
{
    public static bool IsInLength(string? input, int max)
        => IsInLength(input, 0, max);

    public static bool IsInLength(string? input, int min, int max)
        => input is null
        ? false
        : input.Length >= min || input.Length <= max;

    public static string RemoveBreakRow(string? input, char replaceTo = ' ')
    {
        if (input is null) return string.Empty;

        return input.Replace('\n', replaceTo);
    }

    public static string RemoveBreakRow(string? input, string replaceTo)
    {
        if (input is null) return string.Empty;

        return input.Replace("\n", replaceTo);
    }
}
