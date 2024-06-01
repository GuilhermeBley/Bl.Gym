using Bl.Gym.TrainingApi.Application.Providers;
using Bl.Gym.TrainingApi.Application.Repositories;
using Bl.Gym.TrainingApi.Application.Services;
using Bl.Gym.TrainingApi.Domain.Entities.Training;

namespace Bl.Gym.TrainingApi.Application.Commands.Training.CreateUserTraining;

/// <summary>
/// Handler to add a new training sheet to a specific student. 
/// Only the instructor can attribute the training sheet to the 
/// users, and the instructor and his students need to be 
/// registered at the same gym.
/// </summary>
public class CreateTrainingToStudentHandler
    : IRequestHandler<CreateTrainingToStudentRequest, CreateTrainingToStudentResponse>
{
    private readonly IIdentityProvider _identityProvider;
    private readonly TrainingContext _trainingContext;
    private readonly GymRoleCheckerService _gymChecker;
    private readonly ILogger<CreateTrainingToStudentHandler> _logger;

    public async Task<CreateTrainingToStudentResponse> Handle(
        CreateTrainingToStudentRequest request,
        CancellationToken cancellationToken)
    {
        var user = await _identityProvider.GetCurrentAsync(cancellationToken);

        user.ThrowIfDoesntContainRole(Domain.Security.UserClaim.ManageTraining);

        await _gymChecker.ThrowIfUserIsntInTheGymAsync(user.RequiredUserId(), request.GymId, cancellationToken);

        if (!await _gymChecker.UserIsInTheGymAsync(request.StudentId, request.GymId, cancellationToken))
            throw CommonCoreException.CreateByCode(CoreExceptionCode.UserIsntMemberOfThisGym);

        var userAlreadyContainsAnActiveTrainingInThisGym
            = await _trainingContext
            .UserTrainings
            .AsNoTracking()
            .Where(u => u.StudentId == request.StudentId
                && u.GymId == request.GymId
                && u.Status == Domain.Enum.UserTrainingStatus.InProgress)
            .AnyAsync(cancellationToken);

        if (userAlreadyContainsAnActiveTrainingInThisGym)
            throw CoreException.CreateByCode(CoreExceptionCode.UserAlreadyHasATrainingInProgressInThisGym);

        var studentSheet = UserTrainingSheet.CreateNow(
            request.StudentId,
            request.GymId,
            request.Sets.Select(section =>
            {
                var setEntity = TrainingSection.CreateNew(
                    Guid.NewGuid(),
                    section.MuscularGroup,
                    section.Sets.Select(set =>
                        ExerciseSet.CreateNew(set.Set, set.ExerciseId).RequiredResult))
                .RequiredResult;

                return setEntity;
            }))
            .RequiredResult;


    }
}
