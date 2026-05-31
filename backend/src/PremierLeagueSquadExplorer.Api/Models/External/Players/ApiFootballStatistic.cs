using System.Text.Json.Serialization;

namespace PremierLeagueSquadExplorer.Api.Models.External.Players;

public sealed class ApiFootballStatistic
{
    [JsonPropertyName("team")]
    public ApiFootballStatisticTeam Team { get; init; } = new();

    [JsonPropertyName("league")]
    public ApiFootballLeague League { get; init; } = new();

    [JsonPropertyName("games")]
    public ApiFootballGames Games { get; init; } = new();
}
