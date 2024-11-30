using Bl.Gym.TrainingApi.Application.Model.Training;
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
    private readonly TrainingContext _context;
    private readonly GymRoleCheckerService _gymChecker;
    private readonly ILogger<CreateTrainingToStudentHandler> _logger;

    public CreateTrainingToStudentHandler(
        IIdentityProvider identityProvider, 
        TrainingContext context, 
        GymRoleCheckerService gymChecker, 
        ILogger<CreateTrainingToStudentHandler> logger)
    {
        _identityProvider = identityProvider;
        _context = context;
        _gymChecker = gymChecker;
        _logger = logger;
    }

    public async Task<CreateTrainingToStudentResponse> Handle(
        CreateTrainingToStudentRequest request,
        CancellationToken cancellationToken)
    {
        var user = await _identityProvider.GetCurrentAsync(cancellationToken);

        user.ThrowIfDoesntContainRole(Domain.Security.UserClaim.ManageTraining);

        user.ThrowIfIsnInTheGym(request.GymId);

        if (!await _gymChecker.IsUserInTheGymAsync(request.StudentId, request.GymId, cancellationToken))
            throw CommonCoreException.CreateByCode(CoreExceptionCode.UserIsntMemberOfThisGym);

        var userAlreadyContainsAnActiveTrainingInThisGym
            = await _context
            .UserTrainingSheets
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
            request.Sections.Select(section =>
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

        using var transaction =
            await _context.Database.BeginTransactionAsync(cancellationToken);

        var createdSheetResult = await _context
            .UserTrainingSheets
            .AddAsync(UserTrainingSheetModel.MapFromEntity(studentSheet));

        List<TrainingSectionModel> sectionsCreated = new();
        foreach (var section in studentSheet.Sections)
        {
            var sectionResult = await _context.TrainingSections.AddAsync(
                TrainingSectionModel.MapFromEntity(section, createdSheetResult.Entity.Id));

            sectionsCreated.Add(sectionResult.Entity);
        }

        foreach (var sectionCreated in sectionsCreated)
        {
            var sectionEntity = studentSheet.GetSection(sectionCreated.MuscularGroup);

            await _context.ExerciseSets.AddRangeAsync(
                sectionEntity.Sets.Select(set => ExerciseSetModel.MapFromEntity(set, sectionCreated.Id)));

        }

        await transaction.CommitAsync();
        await _context.SaveChangesAsync();

        return new(createdSheetResult.Entity.Id);
    }
}
