using System.Text.Json.Serialization;

namespace PremierLeagueSquadExplorer.Api.Models.External.Teams;

public sealed class ApiFootballVenue
{
    [JsonPropertyName("id")]
    public int? Id { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("address")]
    public string? Address { get; init; }

    [JsonPropertyName("city")]
    public string? City { get; init; }

    [JsonPropertyName("capacity")]
    public int? Capacity { get; init; }

    [JsonPropertyName("surface")]
    public string? Surface { get; init; }

    [JsonPropertyName("image")]
    public string? Image { get; init; }
}
