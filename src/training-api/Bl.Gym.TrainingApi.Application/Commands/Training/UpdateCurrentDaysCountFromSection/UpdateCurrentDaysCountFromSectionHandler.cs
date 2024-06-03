using Bl.Gym.TrainingApi.Application.Providers;
using Bl.Gym.TrainingApi.Application.Repositories;
using Bl.Gym.TrainingApi.Application.Services;

namespace Bl.Gym.TrainingApi.Application.Commands.Training.UpdateCurrentDaysCountFromSection;

internal class UpdateCurrentDaysCountFromSectionHandler
    : IRequestHandler<UpdateCurrentDaysCountFromSectionRequest, Guid>
{
    private readonly IIdentityProvider _identityProvider;
    private readonly TrainingContext _context;
    private readonly GymRoleCheckerService _gymChecker;
    private readonly ILogger<UpdateCurrentDaysCountFromSectionHandler> _logger;

    public UpdateCurrentDaysCountFromSectionHandler(
        IIdentityProvider identityProvider,
        TrainingContext context,
        GymRoleCheckerService gymChecker,
        ILogger<UpdateCurrentDaysCountFromSectionHandler> logger)
    {
        _identityProvider = identityProvider;
        _context = context;
        _gymChecker = gymChecker;
        _logger = logger;
    }

    public async Task<Guid> Handle(UpdateCurrentDaysCountFromSectionRequest request, CancellationToken cancellationToken)
    {
        var user = await _identityProvider.GetCurrentAsync(cancellationToken);

        user.ThrowIfDoesntContainRole(Domain.Security.UserClaim.SeeTraining);

        await _gymChecker.ThrowIfUserIsntInTheSectionAsync(user.RequiredUserId(), request.SectionId, cancellationToken);

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
            .Where(e => e.Id == request.SectionId)
            .FirstOrDefaultAsync(cancellationToken)
             ?? throw CoreException.CreateByCode(CoreExceptionCode.NotFound);

        var entityToUpdate = sectionFound.MapToEntity();

        entityToUpdate.UpdateCurrentDaysCount(entityToUpdate.CurrentDaysCount)
            .EnsureSuccess();

        var affetedRows = await _context
            .TrainingSections
            .Where(e => e.Id == request.SectionId)
            .Where(e => e.ConcurrencyStamp == entityToUpdate.ConcurrencyStamp)
            .ExecuteUpdateAsync(setter => setter.SetProperty(p => p.CurrentDaysCount, entityToUpdate.CurrentDaysCount));

        if (affetedRows != 0)
            throw CoreException.CreateByCode(CoreExceptionCode.ThisEntityWasAlreadyUpdateByAnotherSource);

        return entityToUpdate.Id;
    }
}
