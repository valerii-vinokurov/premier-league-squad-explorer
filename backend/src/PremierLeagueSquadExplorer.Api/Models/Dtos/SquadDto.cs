namespace PremierLeagueSquadExplorer.Api.Models.Dtos;

public sealed class SquadDto
{
    public TeamDto Team { get; init; } = new();

    public IReadOnlyCollection<PlayerDto> Players { get; init; } = [];

    public SquadMetadataDto Metadata { get; init; } = new();
}