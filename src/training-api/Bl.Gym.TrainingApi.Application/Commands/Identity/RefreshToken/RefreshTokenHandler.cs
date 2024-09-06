
using Bl.Gym.TrainingApi.Application.Repositories;
using Bl.Gym.TrainingApi.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Bl.Gym.TrainingApi.Application.Commands.Identity.RefreshToken;

public class RefreshTokenHandler
    : IRequestHandler<RefreshTokenRequest, RefreshTokenResponse>
{
    private readonly TrainingContext _context;
    private readonly ILogger<RefreshTokenHandler> _logger;

    public RefreshTokenHandler(TrainingContext context, 
        ILogger<RefreshTokenHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<RefreshTokenResponse> Handle(
        RefreshTokenRequest request, 
        CancellationToken cancellationToken)
    {
        var entity = RefreshAuthentication.CreateWithDefaultExpiration(
            request.UserId)
            .RequiredResult;

        var refreshTokenFound
            = await _context
            .RefreshAuthentications
            .AsNoTracking()
            .Where(e => e.UserId == request.UserId && e.RefreshToken == request.RefreshToken)
            .FirstOrDefaultAsync(cancellationToken);

        if (refreshTokenFound is null)
        {
            _logger.LogTrace("User or refresh token were not found {0}-{1}.", request.UserId, request.RefreshToken);
            throw new UnauthorizedCoreException();
        }

        var updatedRows =
            await _context
            .RefreshAuthentications
            .Where(e => e.UserId == refreshTokenFound.UserId)
            .Where(e => e.ConcurrencyStamp == refreshTokenFound.ConcurrencyStamp)
            .ExecuteUpdateAsync(setter => setter
                .SetProperty(e => e.RefreshTokenExpiration, entity.RefreshTokenExpiration)
                .SetProperty(e => e.RefreshToken, entity.RefreshToken)
                .SetProperty(e => e.ConcurrencyStamp, Guid.NewGuid())
                .SetProperty(e => e.UpdatedAt, entity.UpdatedAt));

        if (updatedRows < 1)
        {
            _logger.LogTrace("Failed to update refresh token {0}.", request.UserId);
            throw new UnauthorizedCoreException();
        }


        var userFound = await _context
            .Users
            .AsNoTracking()
            .Where(e => e.Id == request.UserId)
            .Select(e => new
            {
                e.Id,
                e.UserName,
                e.Email
            })
            .SingleAsync(cancellationToken);

        var claims = (await GetUserClaimAsync(userId: request.UserId, cancellationToken)).ToArray();

        return new
        (
            RefreshToken: entity.RefreshToken,
            Claims: claims,
            Username: userFound.UserName,
            Email: userFound.Email
        );
    }

    private async Task<IEnumerable<Claim>> GetUserClaimAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var claims = await
            (from userRole in _context.UserRoles.AsNoTracking()
             join role in _context.Roles.AsNoTracking()
                 on userRole.RoleId equals role.Id
             join claim in _context.RoleClaims.AsNoTracking()
                 on role.Id equals claim.RoleId
             where userRole.Id == userId
             select new
             {
                 claim.RoleId,
                 claim.ClaimType,
                 claim.ClaimValue
             })
            .ToListAsync(cancellationToken);

        return claims.Select(c =>
            new Claim(c.ClaimType, c.ClaimValue));
    }
}
