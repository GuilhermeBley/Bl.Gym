using System.Security.Claims;

namespace Bl.Gym.TrainingApi.Application.Providers;

public interface ITokenProvider
{
    Task<string> CreateTokenAsync(
        IEnumerable<Claim> claims,
        CancellationToken cancellationToken = default);
}
