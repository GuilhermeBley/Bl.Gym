
namespace Bl.Gym.TrainingApi.Application.Commands.Identity.ChangePassword;

public class ChangePasswordHandler
    : IRequestHandler<ChangePasswordRequest, ChangePasswordResponse>
{
    public Task<ChangePasswordResponse> Handle(
        ChangePasswordRequest request, 
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
