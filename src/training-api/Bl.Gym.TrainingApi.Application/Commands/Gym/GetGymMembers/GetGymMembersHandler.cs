
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

    public GetGymMembersHandler(TrainingContext trainingContext, GymRoleCheckerService gymRoleCheckerService, IIdentityProvider identityProvider)
    {
        _trainingContext = trainingContext;
        _gymRoleCheckerService = gymRoleCheckerService;
        _identityProvider = identityProvider;
    }

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

        var results = await
            (from user in _trainingContext.Users
             join userRole in _trainingContext.UserTrainingRoles
                on user.Id equals userRole.UserId
             join role in _trainingContext.Roles
                on userRole.RoleId equals role.Id
             where userRole.GymGroupId == request.GymId
             select new GetGymMembersItemResponse(
                 /*UserId*/user.Id,
                 /*Email*/user.Email,
                 /*Name*/user.FirstName,
                 /*LastName*/user.LastName,
                 /*RoleName*/ role.Name))
            .ToArrayAsync(cancellationToken);

        return new(results);
    }
}
