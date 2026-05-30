using System.Text.Json.Serialization;

namespace PremierLeagueSquadExplorer.Api.Models.External.Players;

public sealed class ApiFootballStatisticTeam
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("logo")]
    public string? Logo { get; init; }
}