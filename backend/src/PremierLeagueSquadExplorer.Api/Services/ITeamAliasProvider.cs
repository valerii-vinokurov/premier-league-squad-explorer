using PremierLeagueSquadExplorer.Api.Models.TeamLookup;

namespace PremierLeagueSquadExplorer.Api.Services;

public interface ITeamAliasProvider
{
    IReadOnlyCollection<TeamAliasDefinition> GetSupportedTeams();
}