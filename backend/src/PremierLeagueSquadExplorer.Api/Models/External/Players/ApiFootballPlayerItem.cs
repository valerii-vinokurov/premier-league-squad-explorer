using System.Text.Json.Serialization;

namespace PremierLeagueSquadExplorer.Api.Models.External.Players;

public sealed class ApiFootballPlayerItem
{
    [JsonPropertyName("player")]
    public ApiFootballPlayer Player { get; init; } = new();

    [JsonPropertyName("statistics")]
    public List<ApiFootballStatistic> Statistics { get; init; } = [];
}