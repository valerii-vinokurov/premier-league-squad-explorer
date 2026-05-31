using PremierLeagueSquadExplorer.Api.Models.Dtos;

namespace PremierLeagueSquadExplorer.Api.Services;

public interface ISquadService
{
    Task<SquadDto?> GetSquadAsync(string query, CancellationToken cancellationToken = default);
}