using System.Text.Json.Serialization;

namespace PremierLeagueSquadExplorer.Api.Models.External.Players;

public sealed class ApiFootballPlayer
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("firstname")]
    public string? Firstname { get; init; }

    [JsonPropertyName("lastname")]
    public string? Lastname { get; init; }

    [JsonPropertyName("age")]
    public int? Age { get; init; }

    [JsonPropertyName("birth")]
    public ApiFootballBirth? Birth { get; init; }

    [JsonPropertyName("nationality")]
    public string? Nationality { get; init; }

    [JsonPropertyName("height")]
    public string? Height { get; init; }

    [JsonPropertyName("weight")]
    public string? Weight { get; init; }

    [JsonPropertyName("injured")]
    public bool Injured { get; init; }

    [JsonPropertyName("photo")]
    public string? Photo { get; init; }
}