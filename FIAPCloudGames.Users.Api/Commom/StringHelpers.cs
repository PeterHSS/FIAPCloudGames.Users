using FluentValidation;

namespace FIAPCloudGames.Users.Api.Commom;

public static class StringHelpers
{
    public static string OnlyNumbers(this string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;

        return new string(value.Where(char.IsDigit).ToArray());
    }
}