using PremierLeagueSquadExplorer.Api.Constants;

namespace PremierLeagueSquadExplorer.Api.Models.Dtos;

public sealed class SquadMetadataDto
{
    public int LeagueId { get; init; }

    public int Season { get; init; }

    public string Source { get; init; } = FootballProviderNames.ApiFootball;

    public bool Cached { get; init; }
}