namespace PremierLeagueSquadExplorer.Api.Options;

public sealed class FootballApiOptions
{
    public const string SectionName = "FootballApi";

    public string BaseUrl { get; set; } = string.Empty;

    public string ApiKey { get; set; } = string.Empty;

    public int LeagueId { get; set; } = 39;

    public int Season { get; set; } = 2024;
}