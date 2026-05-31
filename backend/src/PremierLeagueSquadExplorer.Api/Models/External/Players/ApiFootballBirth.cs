using System.Text.Json.Serialization;

namespace PremierLeagueSquadExplorer.Api.Models.External.Players;

public sealed class ApiFootballBirth
{
    [JsonPropertyName("date")]
    public string? Date { get; init; }

    [JsonPropertyName("place")]
    public string? Place { get; init; }

    [JsonPropertyName("country")]
    public string? Country { get; init; }
}