using Microsoft.AspNetCore.Mvc;
using PremierLeagueSquadExplorer.Api.Controllers;
using PremierLeagueSquadExplorer.Api.Models.Dtos;
using PremierLeagueSquadExplorer.Api.Tests.Fakers;

namespace PremierLeagueSquadExplorer.Api.Tests.Controllers;

public sealed class TeamsControllerTests
{
    [Fact]
    public async Task GetTeamsAsync_ShouldReturnOkWithTeams()
    {
        var teams = new[]
        {
            new TeamDto
            {
                Id = 48,
                Name = "West Ham United",
                ProviderName = "West Ham",
                LogoUrl = "https://example.test/west-ham.png"
            }
        };

        var controller = new TeamsController(new FakeTeamResolverService(teams: teams));

        var result = await controller.GetTeamsAsync(CancellationToken.None);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var value = Assert.IsAssignableFrom<IReadOnlyCollection<TeamDto>>(okResult.Value);

        var team = Assert.Single(value);

        Assert.Equal(48, team.Id);
        Assert.Equal("West Ham United", team.Name);
        Assert.Equal("West Ham", team.ProviderName);
        Assert.Equal("https://example.test/west-ham.png", team.LogoUrl);
    }

    [Fact]
    public async Task GetTeamsAsync_ShouldReturnOkWithEmptyCollection_WhenNoTeamsAreAvailable()
    {
        var controller = new TeamsController(new FakeTeamResolverService(teams: []));

        var result = await controller.GetTeamsAsync(CancellationToken.None);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var value = Assert.IsAssignableFrom<IReadOnlyCollection<TeamDto>>(okResult.Value);

        Assert.Empty(value);
    }

    [Fact]
    public async Task GetTeamsAsync_ShouldReturnOkWithEmptyCollection_WhenServiceReturnsNullDefensively()
    {
        var controller = new TeamsController(new FakeTeamResolverService(null));

        var result = await controller.GetTeamsAsync(CancellationToken.None);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var value = Assert.IsAssignableFrom<IReadOnlyCollection<TeamDto>>(okResult.Value);

        Assert.Empty(value);
    }
}