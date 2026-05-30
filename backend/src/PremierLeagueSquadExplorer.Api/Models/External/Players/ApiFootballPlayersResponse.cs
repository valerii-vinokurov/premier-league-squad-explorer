using System.Text.Json;
using System.Text.Json.Serialization;

namespace PremierLeagueSquadExplorer.Api.Models.External.Players;

public sealed class ApiFootballPlayersResponse
{
    [JsonPropertyName("get")]
    public string? Get { get; init; }

    [JsonPropertyName("parameters")]
    public JsonElement Parameters { get; init; }

    [JsonPropertyName("errors")]
    public JsonElement Errors { get; init; }

    [JsonPropertyName("results")]
    public int Results { get; init; }

    [JsonPropertyName("paging")]
    public ApiFootballPaging Paging { get; init; } = new();

    [JsonPropertyName("response")]
    public List<ApiFootballPlayerItem> Response { get; init; } = [];
}