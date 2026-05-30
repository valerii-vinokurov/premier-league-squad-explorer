using System.Text.Json.Serialization;

namespace PremierLeagueSquadExplorer.Api.Models.External.Players;

public sealed class ApiFootballLeague
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("country")]
    public string? Country { get; init; }

    [JsonPropertyName("logo")]
    public string? Logo { get; init; }

    [JsonPropertyName("flag")]
    public string? Flag { get; init; }

    [JsonPropertyName("season")]
    public int Season { get; init; }
}