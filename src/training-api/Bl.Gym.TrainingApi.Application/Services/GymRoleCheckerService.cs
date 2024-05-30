using Bl.Gym.TrainingApi.Application.Repositories;

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

    public async Task ThrowIfUserIsntInTheGymAsync(Guid userId, Guid gymId, CancellationToken cancellationToken = default)
    {
        var contains = await UserIsInTheGymAsync(userId, gymId, cancellationToken);

        if (!contains)
        {
            throw new ForbbidenCoreException($"User '{userId}' is not member of gym '{gymId}'.");
        }
    }

    public async Task<bool> UserIsInTheGymAsync(Guid userId, Guid gymId, CancellationToken cancellationToken = default)
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
}
