using Microsoft.AspNetCore.Mvc;
using PremierLeagueSquadExplorer.Api.Models.Dtos;
using PremierLeagueSquadExplorer.Api.Services;

namespace PremierLeagueSquadExplorer.Api.Controllers;

[ApiController]
[Route("api/squads")]
public sealed class SquadsController(ISquadService squadService) : ControllerBase
{
    private readonly ISquadService _squadService = squadService;

    [HttpGet]
    [ProducesResponseType(typeof(SquadDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SquadDto>> GetSquadAsync(
        [FromQuery] string? query,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return BadRequest(new ErrorResponseDto
            {
                Code = "INVALID_QUERY",
                Message = "Team name or nickname is required."
            });
        }

        var squad = await _squadService.GetSquadAsync(query, cancellationToken);

        if (squad is null)
        {
            return NotFound(new ErrorResponseDto
            {
                Code = "TEAM_NOT_FOUND",
                Message = "The requested team could not be found."
            });
        }

        return Ok(squad);
    }
}