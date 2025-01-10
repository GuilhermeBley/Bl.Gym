using Bl.Gym.TrainingApi.Application.Providers;
using Bl.Gym.TrainingApi.Application.Repositories;

namespace Bl.Gym.TrainingApi.Application.Commands.Gym.GetCurrentUserGym;

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
        var userEmail = user.RequiredUserEmail();

        if (userId != request.UserId)
            throw CoreException.CreateByCode(CoreExceptionCode.Unauthorized);

        var now = DateTime.UtcNow;

        var gyms = await
            (from gym in _trainingContext.GymGroups.AsNoTracking()
             join userRole in _trainingContext.UserTrainingRoles.AsNoTracking()
                 on new { GymId = gym.Id, UserId = userId } equals new { GymId = userRole.GymGroupId, userRole.UserId }
             join role in _trainingContext.Roles.AsNoTracking()
                 on userRole.RoleId equals role.Id
             join invite in _trainingContext.UserGymInvitations.AsNoTracking()
                on new { GymId = gym.Id, Email = userEmail, Accepted = false } equals new { invite.GymId, Email = invite.UserEmail, invite.Accepted }
            into invites
            from inviteNotRequired in invites.DefaultIfEmpty()
            where (inviteNotRequired.ExpiresAt > now || inviteNotRequired == null)
             select new GetCurrentUserGymResponse(
                 gym.Id,
                 gym.Name,
                 gym.Description,
                 gym.CreatedAt,
                 role.Name,
                 inviteNotRequired.Id,
                 inviteNotRequired != null))
            .ToListAsync(cancellationToken);

        return new GetCurrentUserGymsResponse(gyms);
    }
}
