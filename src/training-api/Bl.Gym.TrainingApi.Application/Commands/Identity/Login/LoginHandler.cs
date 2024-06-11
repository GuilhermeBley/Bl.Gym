
using Bl.Gym.TrainingApi.Application.Providers;
using Bl.Gym.TrainingApi.Application.Repositories;
using System.Security.Claims;

namespace Bl.Gym.TrainingApi.Application.Commands.Identity.Login;

public class LoginHandler
    : IRequestHandler<LoginRequest, LoginResponse>
{
    private readonly ILogger<LoginHandler> _logger;
    private readonly TrainingContext _context;
    private readonly ITokenProvider _tokenProvider;

    public LoginHandler(ILogger<LoginHandler> logger, TrainingContext context, ITokenProvider tokenProvider)
    {
        _logger = logger;
        _context = context;
        _tokenProvider = tokenProvider;
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

        var token
            = await _tokenProvider
            .CreateTokenAsync(
                new[] {
                    Domain.Security.UserClaim.CreateUserEmailClaim(userFound.Email),
                    Domain.Security.UserClaim.CreateUserIdClaim(userFound.Id),
                    Domain.Security.UserClaim.CreateUserNameClaim(userFound.UserName),
                });

        return new(
            Username: userFound.UserName,
            Email: userFound.Email,
            Token: token);
    }
}
