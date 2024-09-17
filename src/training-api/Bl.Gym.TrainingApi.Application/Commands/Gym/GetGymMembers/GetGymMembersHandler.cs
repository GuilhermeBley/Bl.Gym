
namespace Bl.Gym.TrainingApi.Application.Commands.Gym.GetGymMembers;

public class GetGymMembersHandler
    : IRequestHandler<GetGymMembersRequest, GetGymMembersResponse>
{
    public Task<GetGymMembersResponse> Handle(GetGymMembersRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
