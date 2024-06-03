using Bl.Gym.TrainingApi.Application.Repositories;
using System;

namespace Bl.Gym.TrainingApi.Application.Services;

public class GymRoleCheckerService
{
    private readonly TrainingContext _context;
    private readonly ILogger<GymRoleCheckerService> _logger;

    public GymRoleCheckerService(TrainingContext context, ILogger<GymRoleCheckerService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task ThrowIfUserIsntInTheSectionAsync(Guid userId, Guid sectionId, CancellationToken cancellationToken = default)
    {
        var contains = await IsUserInTheSectionAsync(userId, sectionId, cancellationToken);

        if (!contains)
        {
            throw new ForbbidenCoreException($"User '{userId}' is not member of section '{sectionId}'.");
        }
    }

    public async Task ThrowIfUserIsntInTheGymAsync(Guid userId, Guid gymId, CancellationToken cancellationToken = default)
    {
        var contains = await IsUserInTheGymAsync(userId, gymId, cancellationToken);

        if (!contains)
        {
            throw new ForbbidenCoreException($"User '{userId}' is not member of gym '{gymId}'.");
        }
    }

    public async Task<bool> IsUserInTheGymAsync(Guid userId, Guid gymId, CancellationToken cancellationToken = default)
    {
        var contains = await _context
            .UserTrainingRoles
            .AsNoTracking()
            .AnyAsync(u =>
                u.Id == userId &&
                u.GymGroupId == gymId,
                cancellationToken);

        return contains;
    }

    public async Task<bool> IsUserInTheSectionAsync(Guid userId, Guid sectionId, CancellationToken cancellationToken = default)
    {
        var contains = await
            (from sheet in _context.UserTrainingSheets.AsNoTracking()
            join section in _context.TrainingSections.AsNoTracking()
                on sheet.Id equals section.UserTrainingSheetId
            join userRole in _context.UserTrainingRoles.AsNoTracking()
                on sheet.GymId equals userRole.GymGroupId
            where section.Id == sectionId && userRole.UserId == userId
             select new { userRole.UserId })
             .AnyAsync(cancellationToken);

        return contains;
    }
}
