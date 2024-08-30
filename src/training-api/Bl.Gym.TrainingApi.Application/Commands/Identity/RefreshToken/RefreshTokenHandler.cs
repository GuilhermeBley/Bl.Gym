
using Bl.Gym.TrainingApi.Domain.Entities.Identity;

namespace Bl.Gym.TrainingApi.Application.Commands.Identity.RefreshToken;

public class RefreshTokenHandler
    : IRequestHandler<RefreshTokenRequest, RefreshTokenResponse>
{
    private readonly static TimeSpan _defaultRefreshTokenExpiration = TimeSpan.FromDays(7);

    public Task<RefreshTokenResponse> Handle(
        RefreshTokenRequest request, 
        CancellationToken cancellationToken)
    {

        var refreshTokenEntity = RefreshAuthentication.Create(
            userFound.Id,
            _defaultRefreshTokenExpiration);

        throw new NotImplementedException();
    }
}
