using System.Text.Json.Serialization;

namespace PremierLeagueSquadExplorer.Api.Models.External.Teams;

public sealed class ApiFootballTeamItem
{
    [JsonPropertyName("team")]
    public ApiFootballTeam Team { get; init; } = new();

    [JsonPropertyName("venue")]
    public ApiFootballVenue? Venue { get; init; }
}