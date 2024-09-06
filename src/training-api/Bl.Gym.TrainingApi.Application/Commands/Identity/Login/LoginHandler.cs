using Bl.Gym.TrainingApi.Application.Model.Identity;
using Bl.Gym.TrainingApi.Application.Repositories;
using Bl.Gym.TrainingApi.Domain.Entities.Identity;
using MediatR;
using System.Collections.Immutable;
using System.Security.Claims;

namespace Bl.Gym.TrainingApi.Application.Commands.Identity.Login;

public class LoginHandler
    : IRequestHandler<LoginRequest, LoginResponse>
{
    private readonly ILogger<LoginHandler> _logger;
    private readonly TrainingContext _context;

    public LoginHandler(ILogger<LoginHandler> logger, TrainingContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<LoginResponse> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        var userFound = await _context
            .Users
            .AsNoTracking()
            .Where(e => e.UserName == request.Login)
            .Select(e => new
            {
                e.Id,
                e.UserName,
                e.Email,
                e.PasswordHash,
                e.PasswordSalt,
                e.AccessFailedCount,
                e.ConcurrencyStamp
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (userFound is null)
        {
            _logger.LogInformation("User was not found with login {0}.", request.Login);
            throw CoreException.CreateByCode(CoreExceptionCode.Unauthorized);
        }

        var isValidPassword = 
            Domain.Security.Sha256Convert.IsValidPassword(request.Password, userFound.PasswordHash, userFound.PasswordSalt);

        if (userFound.AccessFailedCount >= 10)
        {
            _logger.LogInformation("User {0} is locked.", request.Login);
            throw CoreException.CreateByCode(CoreExceptionCode.UserIsLocked);
        }

        if (!isValidPassword)
        {
            await _context
                .Users
                .Where(u => u.Id == userFound.Id)
                .Where(u => u.ConcurrencyStamp == userFound.ConcurrencyStamp)
                .ExecuteUpdateAsync(
                    setter => setter.SetProperty(p => p.AccessFailedCount, userFound.AccessFailedCount + 1));

            _logger.LogInformation("User {0} was found but the password is invalid.", request.Login);
            throw CoreException.CreateByCode(CoreExceptionCode.Unauthorized);
        }

        if (userFound.AccessFailedCount > 0)
        {
            _logger.LogInformation("User {0} typed a valid login; its access will be restored.", request.Login);
            await _context
                .Users
                .Where(u => u.Id == userFound.Id)
                .Where(u => u.ConcurrencyStamp == userFound.ConcurrencyStamp)
                .ExecuteUpdateAsync(
                    setter => setter.SetProperty(p => p.AccessFailedCount, 0));
        }

        var refreshTokenCreated = await CreateRefreshTokenAsync(
            userFound.Id, 
            cancellationToken);

        var userRoles = await GetUserClaimAsync(userFound.Id, cancellationToken);

        var claims =
            new[] {
                Domain.Security.UserClaim.CreateUserEmailClaim(userFound.Email),
                Domain.Security.UserClaim.CreateUserIdClaim(userFound.Id),
                Domain.Security.UserClaim.CreateUserNameClaim(userFound.UserName)
            }.Concat(userRoles);

        return new(
            RefreshToken: refreshTokenCreated.RefreshToken,
            Username: userFound.UserName,
            Email: userFound.Email,
            Claims: claims.ToImmutableArray());
    }

    private async Task<RefreshAuthentication> CreateRefreshTokenAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var entity = RefreshAuthentication.CreateWithDefaultExpiration(
            userId)
            .RequiredResult;

        await _context
            .RefreshAuthentications
            .Where(e => e.UserId == userId)
            .ExecuteDeleteAsync(cancellationToken);

        await _context
            .RefreshAuthentications
            .AddAsync(RefreshAuthenticationModel.MapFromEntity(entity));

        await _context.SaveChangesAsync(cancellationToken);

        return entity;
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
            select new {
                claim.RoleId,
                claim.ClaimType,
                claim.ClaimValue
            })
            .ToListAsync(cancellationToken);

        return claims.Select(c => 
            new Claim(c.ClaimType, c.ClaimValue));
    }
}
