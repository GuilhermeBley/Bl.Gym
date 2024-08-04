
using Bl.Gym.TrainingApi.Application.Model.Identity;
using Bl.Gym.TrainingApi.Application.Providers;
using Bl.Gym.TrainingApi.Application.Repositories;
using Bl.Gym.TrainingApi.Domain.Entities.Identity;

namespace Bl.Gym.TrainingApi.Application.Commands.Identity.ChangePassword;

public class ChangePasswordHandler
    : IRequestHandler<ChangePasswordRequest, ChangePasswordResponse>
{
    private readonly IIdentityProvider _identityProvider;
    private readonly TrainingContext _trainingContext;
    private readonly ILogger<ChangePasswordHandler> _logger;

    public ChangePasswordHandler(
        TrainingContext trainingContext,
        ILogger<ChangePasswordHandler> logger,
        IIdentityProvider identityProvider)
    {
        _trainingContext = trainingContext;
        _logger = logger;
        _identityProvider = identityProvider;
    }

    public async Task<ChangePasswordResponse> Handle(
        ChangePasswordRequest request, 
        CancellationToken cancellationToken)
    {
        var user =
            await _identityProvider.GetCurrentAsync(cancellationToken);

        var userEmailRole = user.RequiredUserEmail();
        user.ThrowIfDoesntContainRole(Domain.Security.UserClaim.ChangePassword);

        if (!userEmailRole.Equals(request.Email))
        {
            _logger.LogTrace("User {0} isn't the same of the request {1}.", userEmailRole, request.Email);
            throw CoreException.CreateByCode(CoreExceptionCode.Forbbiden);
        }

        var userFound = await _trainingContext
            .Users
            .Where(u => u.Email == request.Email)
            .Select(u => new { u.Id, u.ConcurrencyStamp })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw CoreException.CreateByCode(CoreExceptionCode.Forbbiden);

        var hashResult = User.CreatePasswordResult(request.NewPassword).RequiredResult;

        var editedCount = await _trainingContext
            .Users
            .Where(u => u.Id == userFound.Id && u.ConcurrencyStamp == userFound.ConcurrencyStamp)
            .ExecuteUpdateAsync(u => 
                u.SetProperty(p => p.PasswordHash, hashResult.HashPassword)
                .SetProperty(p => p.PasswordSalt, hashResult.Salt)
                .SetProperty(p => p.ConcurrencyStamp, Guid.NewGuid()));

        await _trainingContext.SaveChangesAsync();

        if (editedCount == 0)
            throw CoreException.CreateByCode(CoreExceptionCode.Conflict);

        return new(userFound.Id);
    }
}
