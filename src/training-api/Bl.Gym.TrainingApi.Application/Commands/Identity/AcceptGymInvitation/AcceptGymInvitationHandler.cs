
namespace Bl.Gym.TrainingApi.Application.Commands.Identity.AcceptGymInvitation;

public class AcceptGymInvitationHandler
    : IRequestHandler<AcceptGymInvitationRequest, AcceptGymInvitationResponse>
{
    public Task<AcceptGymInvitationResponse> Handle(
        AcceptGymInvitationRequest request, 
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}