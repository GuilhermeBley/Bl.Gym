
namespace Bl.Gym.TrainingApi.Application.Commands.Identity.SendGymInvitationToUser;

public class SendGymInvitationToUserHandler
    : IRequestHandler<SendGymInvitationToUserRequest, SendGymInvitationToUserResponse>
{
    public Task<SendGymInvitationToUserResponse> Handle(
        SendGymInvitationToUserRequest request, 
        CancellationToken cancellationToken)
    {
        
    }
}
