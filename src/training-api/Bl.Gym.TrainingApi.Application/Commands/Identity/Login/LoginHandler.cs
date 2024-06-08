
using Bl.Gym.TrainingApi.Application.Providers;
using Bl.Gym.TrainingApi.Application.Repositories;

namespace Bl.Gym.TrainingApi.Application.Commands.Identity.Login;

public class LoginHandler
    : IRequestHandler<LoginRequest, LoginResponse>
{
    private readonly ILogger<LoginHandler> _logger;
    private readonly TrainingContext _context;
    private readonly ITokenProvider _tokenProvider;

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
                e.PasswordSalt
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (userFound is null)
        {
            _logger.LogInformation("", )
            throw CoreException.CreateByCode(CoreExceptionCode.Unauthorized);
        }

        var isValidPassword = 
            Domain.Security.Sha256Convert.IsValidPassword(request.Password, userFound.PasswordHash, userFound.PasswordSalt);

        if (!isValidPassword)
        {

        }
    }
}
