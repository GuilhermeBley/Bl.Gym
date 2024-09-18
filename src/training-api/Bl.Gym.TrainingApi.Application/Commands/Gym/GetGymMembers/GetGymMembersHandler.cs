
using Bl.Gym.TrainingApi.Application.Providers;
using Bl.Gym.TrainingApi.Application.Repositories;
using Bl.Gym.TrainingApi.Application.Services;

namespace Bl.Gym.TrainingApi.Application.Commands.Gym.GetGymMembers;

public class GetGymMembersHandler
    : IRequestHandler<GetGymMembersRequest, GetGymMembersResponse>
{
    private readonly TrainingContext _trainingContext;
    private readonly GymRoleCheckerService _gymRoleCheckerService;
    private readonly IIdentityProvider _identityProvider;

    public async Task<GetGymMembersResponse> Handle(
        GetGymMembersRequest request, 
        CancellationToken cancellationToken)
    {

        var currentUser = await _identityProvider.GetCurrentAsync(cancellationToken);

        //
        // Only 'instructor'(or above) can access the list of students
        //
        await _gymRoleCheckerService.ThrowIfUserDoesNotContainRoleInGymAsync(
            currentUser.RequiredUserId(),
            request.GymId,
            role: Domain.Security.UserClaim.ManageTraining.Value,
            cancellationToken);


    }
}
