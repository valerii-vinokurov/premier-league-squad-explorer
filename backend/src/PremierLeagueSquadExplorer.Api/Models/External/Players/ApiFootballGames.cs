using System.Text.Json.Serialization;

namespace PremierLeagueSquadExplorer.Api.Models.External.Players;

public sealed class ApiFootballGames
{
    [JsonPropertyName("appearences")]
    public int? Appearances { get; init; }

    [JsonPropertyName("lineups")]
    public int? Lineups { get; init; }

    [JsonPropertyName("minutes")]
    public int? Minutes { get; init; }

    [JsonPropertyName("number")]
    public int? Number { get; init; }

    [JsonPropertyName("position")]
    public string? Position { get; init; }

    [JsonPropertyName("rating")]
    public string? Rating { get; init; }

    [JsonPropertyName("captain")]
    public bool Captain { get; init; }
}