
using Bl.Gym.TrainingApi.Application.Providers;
using Bl.Gym.TrainingApi.Application.Repositories;
using Bl.Gym.TrainingApi.Application.Services;

namespace Bl.Gym.TrainingApi.Application.Commands.Training.GetAvailableExercises;

public class GetAvailableExercisesHandler
    : IRequestHandler<GetAvailableExercisesRequest, GetAvailableExercisesResponse>
{
    private readonly ILogger<GetAvailableExercisesHandler> _logger;
    private readonly TrainingContext _trainingContext;
    private readonly IIdentityProvider _identityProvider;
    private readonly GymRoleCheckerService _gymRoleCheckerService;

    public async Task<GetAvailableExercisesResponse> Handle(
        GetAvailableExercisesRequest request, 
        CancellationToken cancellationToken)
    {
        var currentUser = await _identityProvider.GetCurrentAsync(cancellationToken);

        await _gymRoleCheckerService.ThrowIfUserIsntInTheGymAsync(
            currentUser.RequiredUserId(),
            request.GymId,
            cancellationToken);

        return 
    }
}
