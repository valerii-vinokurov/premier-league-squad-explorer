using System.Text.RegularExpressions;

namespace PremierLeagueSquadExplorer.Api.Services;

public static partial class TeamNameNormalizer
{
    public static string Normalize(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;

        var normalized = value
            .Trim()
            .ToLowerInvariant()
            .Replace("&", " and ");

        normalized = UnsupportedCharactersRegex().Replace(normalized, " ");
        normalized = WhitespaceRegex().Replace(normalized, " ");

        return normalized.Trim();
    }

    [GeneratedRegex(@"\s+")]
    private static partial Regex WhitespaceRegex();

    [GeneratedRegex(@"[^a-z0-9\s]")]
    private static partial Regex UnsupportedCharactersRegex();
}