using PremierLeagueSquadExplorer.Api.Models.Dtos;

namespace PremierLeagueSquadExplorer.Api.Tests.TestData;

internal static class DtoTestData
{
    internal static TeamDto CreateWestHamTeam()
        => new()
        {
            Id = 48,
            Name = "West Ham United",
            ProviderName = "West Ham",
            LogoUrl = "https://example.test/west-ham.png"
        };
}