using System.Text.Json.Serialization;

namespace PremierLeagueSquadExplorer.Api.Models.External.Teams;

public sealed class ApiFootballTeam
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("code")]
    public string? Code { get; init; }

    [JsonPropertyName("country")]
    public string? Country { get; init; }

    [JsonPropertyName("founded")]
    public int? Founded { get; init; }

    [JsonPropertyName("national")]
    public bool National { get; init; }

    [JsonPropertyName("logo")]
    public string? Logo { get; init; }
}
