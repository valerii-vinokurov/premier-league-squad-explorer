using PremierLeagueSquadExplorer.Api.Models.Dtos;

namespace PremierLeagueSquadExplorer.Api.Services;

public interface ITeamResolverService
{
    Task<IReadOnlyCollection<TeamDto>> GetTeamsAsync(CancellationToken cancellationToken = default);

    Task<TeamDto?> ResolveAsync(string query, CancellationToken cancellationToken = default);
}