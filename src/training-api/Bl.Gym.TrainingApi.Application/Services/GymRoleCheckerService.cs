using Bl.Gym.TrainingApi.Application.Repositories;
using System;
using System.Security.Claims;

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

    public Task ThrowIfUserDoesNotContainRoleInGymAsync(
        Guid userId,
        Guid gymId,
        string role,
        CancellationToken cancellationToken = default)
        => ThrowIfUserDoesNotContainClaimInGymAsync(
            userId: userId,
            gymId: gymId,
            claimType: Domain.Security.UserClaim.DEFAULT_ROLE,
            claimValue: role,
            cancellationToken);

    public async Task ThrowIfUserDoesNotContainClaimInGymAsync(
        Guid userId, 
        Guid gymId,
        string claimType,
        string claimValue,
        CancellationToken cancellationToken = default)
    {
        var claims = await GetUserClaimsByGymAsync(userId, gymId, cancellationToken);

        if (!claims.Any(c => c.Type.Equals(claimType, StringComparison.OrdinalIgnoreCase) &&
                c.Value.Equals(claimValue, StringComparison.OrdinalIgnoreCase)))
        {
            throw new ForbbidenCoreException($"User '{userId}' is not member of Gym '{gymId}'.");
        }
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
                u.UserId == userId &&
                u.GymGroupId == gymId,
                cancellationToken);

        return contains;
    }

    public async Task<Claim[]> GetUserClaimsByGymAsync(Guid userId, Guid gymId, CancellationToken cancellationToken = default)
    {
        var claims =
            await (
            from userRole in _context.UserTrainingRoles.AsNoTracking()
            join role in _context.Roles.AsNoTracking()
                on userRole.RoleId equals role.Id
            join claim in _context.RoleClaims.AsNoTracking()
                on role.Id equals claim.RoleId
            where userRole.Id == userId && userRole.GymGroupId == gymId
            select new Claim(claim.ClaimType, claim.ClaimValue))
            .ToListAsync(cancellationToken);

        return claims.ToArray();
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
