using PremierLeagueSquadExplorer.Api.Models.TeamLookup;

namespace PremierLeagueSquadExplorer.Api.Tests.TestData;

internal static class TeamAliasTestData
{
    internal static IReadOnlyCollection<TeamAliasDefinition> CreateWestHamAliases()
        =>
        [
            new TeamAliasDefinition(
                OfficialName: "West Ham United",
                ProviderName: "West Ham",
                Aliases:
                [
                    "West Ham",
                    "The Hammers",
                    "Hammers"
                ])
        ];
}