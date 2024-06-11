using Bl.Gym.TrainingApi.Application.Providers;
using Bl.Gym.TrainingApi.Application.Repositories;
using System.Linq;
using System.Security.Claims;

namespace Bl.Gym.TrainingApi.Application.Commands.Identity.LoginToSpecificGym;

public class LoginToSpecificGymHandler
    : IRequestHandler<LoginToSpecificGymRequest, LoginToSpecificGymResponse>
{
    private static readonly string[] _userClaimTypes
        = new[]
        {
            Domain.Security.UserClaim.DEFAULT_USER_EMAIL,
            Domain.Security.UserClaim.DEFAULT_USER_NAME,
            Domain.Security.UserClaim.DEFAULT_USER_ID,
        };

    private readonly ILogger<LoginToSpecificGymHandler> _logger;
    private readonly TrainingContext _context;
    private readonly ITokenProvider _tokenProvider;
    private readonly IIdentityProvider _identityProvider;

    public LoginToSpecificGymHandler(
        ILogger<LoginToSpecificGymHandler> logger, 
        TrainingContext context, 
        ITokenProvider tokenProvider,
        IIdentityProvider identityProvider)
    {
        _logger = logger;
        _context = context;
        _tokenProvider = tokenProvider;
        _identityProvider = identityProvider;
    }

    public async Task<LoginToSpecificGymResponse> Handle(
        LoginToSpecificGymRequest request, 
        CancellationToken cancellationToken)
    {
        var user = await _identityProvider.GetCurrentAsync(cancellationToken);

        var userId = user.RequiredUserId();

        var gymId = user.GetGymId();

        if (gymId is not null)
            throw CoreException.CreateByCode(CoreExceptionCode.UserAlreadyLoggedInGym);

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

        var gymSecurityClaims = gymClaims
            .Select(c => new Claim(c.ClaimType, c.ClaimValue))
            .ToArray()
            .Concat(new[] 
            { 
                Domain.Security.UserClaim.CreateGymClaim(request.GymId)
            });
        
        var token = await _tokenProvider.CreateTokenAsync(
            gymSecurityClaims
                .Concat(GetRequiredClaimsFromUser(user)));

        return new LoginToSpecificGymResponse(
            user.RequiredUserName(),
            user.RequiredUserEmail(),
            request.GymId,
            token);
    }

    private static Claim[] GetRequiredClaimsFromUser(ClaimsPrincipal identity)
    {
        return
            identity.Claims
            .Where(userClaim =>
                _userClaimTypes.Any(requiredType => userClaim.ValueType.Equals(requiredType, StringComparison.OrdinalIgnoreCase)))
            .Select(c => c.Clone())
            .ToArray();
    }
}
