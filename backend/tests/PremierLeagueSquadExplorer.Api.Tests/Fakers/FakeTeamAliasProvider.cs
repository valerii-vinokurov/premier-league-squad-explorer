using PremierLeagueSquadExplorer.Api.Models.TeamLookup;
using PremierLeagueSquadExplorer.Api.Services;

namespace PremierLeagueSquadExplorer.Api.Tests.Fakers;

internal sealed class FakeTeamAliasProvider(IReadOnlyCollection<TeamAliasDefinition> supportedTeams) : ITeamAliasProvider
{
    private readonly IReadOnlyCollection<TeamAliasDefinition> _supportedTeams = supportedTeams;

    public IReadOnlyCollection<TeamAliasDefinition> GetSupportedTeams()
        => _supportedTeams;
}