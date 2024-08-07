
using Bl.Gym.TrainingApi.Application.Providers;
using Bl.Gym.TrainingApi.Application.Repositories;

namespace Bl.Gym.TrainingApi.Application.Commands.Training.GetCurrentUserGym;

public class GetCurrentUserGymsHandler
    : IRequestHandler<GetCurrentUserGymsRequest, GetCurrentUserGymsResponse>
{
    private readonly TrainingContext _trainingContext;
    private readonly IIdentityProvider _identityProvider;

    public GetCurrentUserGymsHandler(TrainingContext trainingContext, IIdentityProvider identityProvider)
    {
        _trainingContext = trainingContext;
        _identityProvider = identityProvider;
    }

    public async Task<GetCurrentUserGymsResponse> Handle(
        GetCurrentUserGymsRequest request, 
        CancellationToken cancellationToken)
    {
        var user = await _identityProvider.GetCurrentAsync(cancellationToken);

        var userId = user.RequiredUserId();

        if (userId != request.UserId)
            throw CoreException.CreateByCode(CoreExceptionCode.Unauthorized);

        var gyms = await
            (from gym in _trainingContext.GymGroups.AsNoTracking()
             join userRole in _trainingContext.UserTrainingRoles.AsNoTracking()
                 on new { GymId = gym.Id, UserId = userId } equals new { GymId = userRole.GymGroupId, UserId = userRole.UserId }
             join role in _trainingContext.Roles.AsNoTracking()
                 on userRole.RoleId equals role.Id
             select new GetCurrentUserGymResponse(
                 gym.Id,
                 gym.Name,
                 gym.Description,
                 gym.CreatedAt,
                 role.Name))
            .ToListAsync(cancellationToken);

        return new GetCurrentUserGymsResponse(gyms);
    }
}
