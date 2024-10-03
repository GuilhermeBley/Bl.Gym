
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

    public GetAvailableExercisesHandler(
        ILogger<GetAvailableExercisesHandler> logger, 
        TrainingContext trainingContext, 
        IIdentityProvider identityProvider, 
        GymRoleCheckerService gymRoleCheckerService)
    {
        _logger = logger;
        _trainingContext = trainingContext;
        _identityProvider = identityProvider;
        _gymRoleCheckerService = gymRoleCheckerService;
    }

    public async Task<GetAvailableExercisesResponse> Handle(
        GetAvailableExercisesRequest request, 
        CancellationToken cancellationToken)
    {
        if (request.Skip < 0 || request.Take < 0 || request.Take > 1000)
            throw CoreException.CreateByCode(CoreExceptionCode.BadRequest);

        var currentUser = await _identityProvider.GetCurrentAsync(cancellationToken);

        //
        // Only 'instructor' can access the entire list of exercises
        //
        await _gymRoleCheckerService.ThrowIfUserDoesNotContainRoleInGymAsync(
            currentUser.RequiredUserId(),
            request.GymId,
            role: Domain.Security.UserClaim.ManageTraining.Value,
            cancellationToken);

        var exercises = await
            _trainingContext
            .Exercises
            .AsNoTracking()
            .Select(e => new GetAvailableExercisesItemResponse(e.Id, e.Title, e.Description))
            .Skip(request.Skip)
            .Take(request.Take)
            .ToListAsync(cancellationToken);

        return new GetAvailableExercisesResponse(exercises.ToArray());
    }
}
