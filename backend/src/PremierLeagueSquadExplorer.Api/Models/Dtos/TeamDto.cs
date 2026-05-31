namespace PremierLeagueSquadExplorer.Api.Models.Dtos;

public sealed class TeamDto
{
    public int Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string? ProviderName { get; init; }

    public string? LogoUrl { get; init; }
}