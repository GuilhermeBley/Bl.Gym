using System.Security.Claims;

namespace Bl.Gym.TrainingApi.Api.Providers;

internal class ContextIdentityProvider
    : Application.Providers.IIdentityProvider
{
    public ClaimsPrincipal ClaimPrincipal = new();

    public Task<ClaimsPrincipal> GetCurrentAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return Task.FromResult(ClaimPrincipal);
    }
}
