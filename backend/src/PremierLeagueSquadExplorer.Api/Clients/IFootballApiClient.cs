using PremierLeagueSquadExplorer.Api.Models.External.Players;
using PremierLeagueSquadExplorer.Api.Models.External.Teams;

namespace PremierLeagueSquadExplorer.Api.Clients;

public interface IFootballApiClient
{
    Task<IReadOnlyCollection<ApiFootballTeamItem>> GetPremierLeagueTeamsAsync(
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<ApiFootballPlayerItem>> GetPlayersByTeamAsync(
        int teamId,
        CancellationToken cancellationToken = default);
}