
namespace Bl.Gym.TrainingApi.Application.Commands.Identity.RefreshToken;

public class RefreshTokenHandler
    : IRequestHandler<RefreshTokenRequest, RefreshTokenResponse>
{
    public Task<RefreshTokenResponse> Handle(
        RefreshTokenRequest request, 
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
