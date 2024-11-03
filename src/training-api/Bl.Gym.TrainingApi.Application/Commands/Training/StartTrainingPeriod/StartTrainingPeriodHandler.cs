
using Bl.Gym.TrainingApi.Application.Model.Training;
using Bl.Gym.TrainingApi.Application.Providers;
using Bl.Gym.TrainingApi.Application.Repositories;
using Bl.Gym.TrainingApi.Application.Services;
using Bl.Gym.TrainingApi.Domain.Entities.Training;

namespace Bl.Gym.TrainingApi.Application.Commands.Training.StartTrainingPeriod;

public class StartTrainingPeriodHandler
    : IRequestHandler<StartTrainingPeriodRequest, StartTrainingPeriodResponse>
{
    private readonly TrainingContext _trainingContext;
    private readonly GymRoleCheckerService _gymRoleCheckerService;
    private readonly IIdentityProvider _identityProvider;
    private readonly ILogger<StartTrainingPeriodHandler> _logger;

    public StartTrainingPeriodHandler(
        TrainingContext trainingContext, 
        GymRoleCheckerService gymRoleCheckerService, 
        IIdentityProvider identityProvider, 
        ILogger<StartTrainingPeriodHandler> logger)
    {
        _trainingContext = trainingContext;
        _gymRoleCheckerService = gymRoleCheckerService;
        _identityProvider = identityProvider;
        _logger = logger;
    }

    public async Task<StartTrainingPeriodResponse> Handle(
        StartTrainingPeriodRequest request, 
        CancellationToken cancellationToken)
    {
        var user = await _identityProvider.GetCurrentAsync(cancellationToken);

        var userId = user.RequiredUserId();

        var containsLastNotFinished = await _trainingContext
            .TrainingsPeriod
            .AsNoTracking()
            .Where(e => e.UserId == userId)
            .Where(e => e.EndedAt == null)
            .AnyAsync(cancellationToken);

        if (containsLastNotFinished)
        {
            _logger.LogInformation("There is already an opened training period to user {0}.", userId);
            throw CoreException.CreateByCode(CoreExceptionCode.Conflict);
        }

        await _gymRoleCheckerService.ThrowIfUserIsntInTheSectionAsync(userId, request.TrainingSectionId, cancellationToken);

        var entity = TrainingUserPeriod.Create(
            id: Guid.NewGuid(),
            userId: userId,
            sectionId: request.TrainingSectionId,
            startedAt: DateTime.UtcNow,
            endedAt: null,
            observation: string.Empty,
            createdAt: DateTime.UtcNow,
            updatedAt: DateTime.UtcNow)
            .RequiredResult;

        var entityResult = await _trainingContext
            .TrainingsPeriod
            .AddAsync(TrainingUserPeriodModel.MapFromEntity(entity), cancellationToken);

        await _trainingContext.SaveChangesAsync();

        return new(entityResult.Entity.Id);
    }
}
