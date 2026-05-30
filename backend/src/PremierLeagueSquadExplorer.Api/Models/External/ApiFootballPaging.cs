using System.Text.Json.Serialization;

namespace PremierLeagueSquadExplorer.Api.Models.External;

public sealed class ApiFootballPaging
{
    [JsonPropertyName("current")]
    public int Current { get; init; }

    [JsonPropertyName("total")]
    public int Total { get; init; }
}