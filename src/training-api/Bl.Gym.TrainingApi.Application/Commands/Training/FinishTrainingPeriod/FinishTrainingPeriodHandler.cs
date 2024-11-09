
using Bl.Gym.TrainingApi.Application.Providers;
using Bl.Gym.TrainingApi.Application.Repositories;
using Bl.Gym.TrainingApi.Application.Services;

namespace Bl.Gym.TrainingApi.Application.Commands.Training.FinishTrainingPeriod;

public class FinishTrainingPeriodHandler
    : IRequestHandler<FinishTrainingPeriodRequest, FinishTrainingPeriodResponse>
{
    private readonly IIdentityProvider _identityProvider;
    private readonly TrainingContext _context;
    private readonly GymRoleCheckerService _gymChecker;
    private readonly ILogger<FinishTrainingPeriodHandler> _logger;

    public FinishTrainingPeriodHandler(
        IIdentityProvider identityProvider,
        TrainingContext context,
        GymRoleCheckerService gymChecker,
        ILogger<FinishTrainingPeriodHandler> logger)
    {
        _identityProvider = identityProvider;
        _context = context;
        _gymChecker = gymChecker;
        _logger = logger;
    }

    public async Task<FinishTrainingPeriodResponse> Handle(
        FinishTrainingPeriodRequest request, 
        CancellationToken cancellationToken)
    {
        var user = await _identityProvider.GetCurrentAsync(cancellationToken);

        user.ThrowIfDoesntContainRole(Domain.Security.UserClaim.SeeTraining);

        var periodToUpdate = await _context
            .TrainingsPeriod
            .Where(e => e.Id == request.PeriodId)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw CoreException.CreateByCode(CoreExceptionCode.NotFound);

        var periodEntity = periodToUpdate.MapToEntity();

        periodEntity.Update(periodEntity.StartedAt ?? DateTime.UtcNow, DateTime.UtcNow);

        await _gymChecker.ThrowIfUserIsntInTheSectionAsync(user.RequiredUserId(), periodToUpdate.SectionId, cancellationToken);

        var sheetFound = await
            (from sheet in _context.UserTrainingSheets.AsNoTracking()
             join section in _context.TrainingSections.AsNoTracking()
                 on sheet.Id equals section.UserTrainingSheetId
             select sheet).FirstOrDefaultAsync(cancellationToken)
             ?? throw CoreException.CreateByCode(CoreExceptionCode.NotFound);

        if (sheetFound.Status != Domain.Enum.UserTrainingStatus.InProgress)
        {
            _logger.LogInformation("Sheet {0} was not started.", sheetFound.Id);
            throw CoreException.CreateByCode(CoreExceptionCode.ThisUserSheetIsntStarted);
        }

        var sectionFound = await _context
            .TrainingSections
            .AsNoTracking()
            .Where(e => e.Id == periodToUpdate.SectionId)
            .FirstOrDefaultAsync(cancellationToken)
             ?? throw CoreException.CreateByCode(CoreExceptionCode.NotFound);

        var entityToUpdate = sectionFound.MapToEntity();

        entityToUpdate.UpdateCurrentDaysCount(entityToUpdate.CurrentDaysCount)
            .EnsureSuccess();

        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        var affetedRows = await _context
            .TrainingSections
            .Where(e => e.Id == periodToUpdate.SectionId)
            .Where(e => e.ConcurrencyStamp == entityToUpdate.ConcurrencyStamp)
            .ExecuteUpdateAsync(setter => setter.SetProperty(p => p.CurrentDaysCount, entityToUpdate.CurrentDaysCount));

        periodToUpdate.EndedAt = periodEntity.EndedAt;
        periodToUpdate.UpdatedAt = periodEntity.UpdatedAt;
        periodToUpdate.StartedAt = periodEntity.StartedAt;
        periodToUpdate.Observation = periodEntity.Observation;

        if (affetedRows != 0)
            throw CoreException.CreateByCode(CoreExceptionCode.ThisEntityWasAlreadyUpdateByAnotherSource);

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        return new();
    }
}
