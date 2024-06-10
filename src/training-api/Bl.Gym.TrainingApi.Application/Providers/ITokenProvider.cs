using System.Security.Claims;

namespace Bl.Gym.TrainingApi.Application.Providers;

public interface ITokenProvider
{
    Task<string> GetTokenAsync(
        IEnumerable<Claim> claims,
        CancellationToken cancellationToken = default);
}
