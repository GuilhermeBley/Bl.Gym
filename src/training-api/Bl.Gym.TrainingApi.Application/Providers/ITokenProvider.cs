namespace Bl.Gym.TrainingApi.Application.Providers;

public interface ITokenProvider
{
    Task<string> GetTokenAsync(
        IEnumerable<(string ClaimType, string ClaimValue)> claims,
        CancellationToken cancellationToken = default);
}
