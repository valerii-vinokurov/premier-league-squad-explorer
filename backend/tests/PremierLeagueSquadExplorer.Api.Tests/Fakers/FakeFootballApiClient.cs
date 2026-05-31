using PremierLeagueSquadExplorer.Api.Clients;
using PremierLeagueSquadExplorer.Api.Models.External.Players;
using PremierLeagueSquadExplorer.Api.Models.External.Teams;

namespace PremierLeagueSquadExplorer.Api.Tests.Fakers;

internal sealed class FakeFootballApiClient(
    IReadOnlyCollection<ApiFootballTeamItem>? teams = null,
    IReadOnlyCollection<ApiFootballPlayerItem>? players = null,
    Exception? playersException = null) : IFootballApiClient
{
    private readonly IReadOnlyCollection<ApiFootballTeamItem> _teams = teams ?? [];
    private readonly IReadOnlyCollection<ApiFootballPlayerItem> _players = players ?? [];
    private readonly Exception? _playersException = playersException;

    public int GetPremierLeagueTeamsCallCount { get; private set; }

    public int GetPlayersByTeamCallCount { get; private set; }

    public Task<IReadOnlyCollection<ApiFootballTeamItem>> GetPremierLeagueTeamsAsync(
        CancellationToken cancellationToken = default)
    {
        GetPremierLeagueTeamsCallCount++;

        return Task.FromResult(_teams);
    }

    public Task<IReadOnlyCollection<ApiFootballPlayerItem>> GetPlayersByTeamAsync(
        int teamId,
        CancellationToken cancellationToken = default)
    {
        GetPlayersByTeamCallCount++;

        if (_playersException is not null)
            throw _playersException;

        return Task.FromResult(_players);
    }
}