using PremierLeagueSquadExplorer.Api.Models.Dtos;
using PremierLeagueSquadExplorer.Api.Services;

namespace PremierLeagueSquadExplorer.Api.Tests.Fakers;

internal sealed class FakeSquadService : ISquadService
{
    public Task<SquadDto?> GetSquadAsync(
        string query,
        CancellationToken cancellationToken = default)
            => Task.FromResult<SquadDto?>(null);
}
