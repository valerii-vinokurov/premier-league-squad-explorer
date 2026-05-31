using PremierLeagueSquadExplorer.Api.Models.Dtos;
using PremierLeagueSquadExplorer.Api.Services;

namespace PremierLeagueSquadExplorer.Api.Tests.Fakers;

internal sealed class FakeTeamResolverService(
    TeamDto? resolvedTeam = null,
    IReadOnlyCollection<TeamDto>? teams = null) : ITeamResolverService
{
    private readonly TeamDto? _resolvedTeam = resolvedTeam;
    private readonly IReadOnlyCollection<TeamDto> _teams = teams ?? (resolvedTeam is null ? [] : [resolvedTeam]);

    public Task<IReadOnlyCollection<TeamDto>> GetTeamsAsync(
        CancellationToken cancellationToken = default)
            => Task.FromResult(_teams);

    public Task<TeamDto?> ResolveAsync(
        string query,
        CancellationToken cancellationToken = default)
            => Task.FromResult(_resolvedTeam);
}