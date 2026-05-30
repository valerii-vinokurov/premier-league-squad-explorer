using Microsoft.AspNetCore.Mvc;
using PremierLeagueSquadExplorer.Api.Models.Dtos;
using PremierLeagueSquadExplorer.Api.Services;

namespace PremierLeagueSquadExplorer.Api.Controllers;

[ApiController]
[Route("api/teams")]
public sealed class TeamsController(ITeamResolverService teamResolverService) : ControllerBase
{
    private readonly ITeamResolverService _teamResolverService = teamResolverService;

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<TeamDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<TeamDto>>> GetTeamsAsync(
        CancellationToken cancellationToken)
    {
        var teams = await _teamResolverService.GetTeamsAsync(cancellationToken);

        return Ok(teams ?? []);
    }
}