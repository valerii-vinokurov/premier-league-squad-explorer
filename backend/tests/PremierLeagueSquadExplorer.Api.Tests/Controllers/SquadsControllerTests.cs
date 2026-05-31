using Microsoft.AspNetCore.Mvc;
using PremierLeagueSquadExplorer.Api.Constants;
using PremierLeagueSquadExplorer.Api.Controllers;
using PremierLeagueSquadExplorer.Api.Models.Dtos;
using PremierLeagueSquadExplorer.Api.Tests.Fakers;

namespace PremierLeagueSquadExplorer.Api.Tests.Controllers;

public sealed class SquadsControllerTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task GetSquadAsync_ShouldReturnBadRequest_WhenQueryIsInvalid(
        string? query)
    {
        var controller = new SquadsController(new FakeSquadService());

        var result = await controller.GetSquadAsync(query, CancellationToken.None);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
        var error = Assert.IsType<ErrorResponseDto>(badRequest.Value);

        Assert.Equal(ErrorCodes.InvalidQuery, error.Code);
        Assert.Equal("Team name or nickname is required.", error.Message);
    }

    [Fact]
    public async Task GetSquadAsync_ShouldReturnNotFound_WhenSquadIsNotResolved()
    {
        var controller = new SquadsController(new FakeSquadService());

        var result = await controller.GetSquadAsync("Unknown Team", CancellationToken.None);

        var notFound = Assert.IsType<NotFoundObjectResult>(result.Result);
        var error = Assert.IsType<ErrorResponseDto>(notFound.Value);

        Assert.Equal(ErrorCodes.TeamNotFound, error.Code);
        Assert.Equal("The requested team could not be found.", error.Message);
    }

    
}