using Bl.Gym.TrainingApi.Application.Providers;
using Bl.Gym.TrainingApi.Application.Repositories;
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Claims;

namespace Bl.Gym.TrainingApi.Application.Commands.Identity.LoginToSpecificGym;

/// <summary>
/// Login to Gym
/// </summary>
public class LoginToSpecificGymHandler
    : IRequestHandler<LoginToSpecificGymRequest, LoginToSpecificGymResponse>
{
    private readonly ILogger<LoginToSpecificGymHandler> _logger;
    private readonly TrainingContext _context;
    private readonly IIdentityProvider _identityProvider;

    public LoginToSpecificGymHandler(
        ILogger<LoginToSpecificGymHandler> logger, 
        TrainingContext context, 
        IIdentityProvider identityProvider)
    {
        _logger = logger;
        _context = context;
        _identityProvider = identityProvider;
    }

    public async Task<LoginToSpecificGymResponse> Handle(
        LoginToSpecificGymRequest request, 
        CancellationToken cancellationToken)
    {
        var user = await _identityProvider.GetCurrentAsync(cancellationToken);

        var userId = user.RequiredUserId();

        var gymClaims = await
            (from gymRole in _context.UserTrainingRoles.AsNoTracking()
             join claim in _context.RoleClaims.AsNoTracking()
                on gymRole.RoleId equals claim.RoleId
             where gymRole.UserId == userId
             where gymRole.GymGroupId == request.GymId
             select new
             {
                 claim.ClaimType,
                 claim.ClaimValue,
             })
             .ToListAsync(cancellationToken);

        if (!gymClaims.Any())
        {
            _logger.LogInformation("User {0} cant access gym {1}.", userId, request.GymId);
            throw CoreException.CreateByCode(CoreExceptionCode.Unauthorized);
        }

        var refreshTokenObj = 
            await _context
            .RefreshAuthentications
            .Where(e => e.UserId == userId)
            .Select(e => new { e.RefreshToken })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new UnauthorizedCoreException("Invalid refresh token.");

        var gymSecurityClaims = gymClaims
            .Select(c => new Claim(c.ClaimType, c.ClaimValue))
            //
            // Adding current user roles
            //
            .Concat(user.Claims)
            .Concat([Domain.Security.UserClaim.CreateGymClaim(request.GymId)]);

        return new LoginToSpecificGymResponse(
            Username: user.RequiredUserName(),
            Email: user.RequiredUserEmail(),
            FirstName: user.RequiredFirstName(),
            LastName: user.RequiredLastName(),
            RefreshToken: refreshTokenObj.RefreshToken,
            GymId: request.GymId,
            Claims: gymSecurityClaims.ToImmutableArray());
    }
}
