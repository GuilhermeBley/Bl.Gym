using System.Security.Claims;

namespace Bl.Gym.TrainingApi.Application.Providers;

public interface IIdentityProvider
{
    Task<ClaimsPrincipal> GetCurrentAsync(CancellationToken cancellationToken = default);
}
