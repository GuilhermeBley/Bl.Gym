
using Bl.Gym.TrainingApi.Application.Providers;
using Bl.Gym.TrainingApi.Application.Repositories;

namespace Bl.Gym.TrainingApi.Application.Commands.Identity.AcceptGymInvitation;

public class AcceptGymInvitationHandler
    : IRequestHandler<AcceptGymInvitationRequest, AcceptGymInvitationResponse>
{
    private readonly TrainingContext _context;
    private readonly ILogger<AcceptGymInvitationRequest> _logger;
    private readonly IIdentityProvider _identityProvider;

    public AcceptGymInvitationHandler(TrainingContext context, ILogger<AcceptGymInvitationRequest> logger, IIdentityProvider identityProvider)
    {
        _context = context;
        _logger = logger;
        _identityProvider = identityProvider;
    }

    public async Task<AcceptGymInvitationResponse> Handle(
        AcceptGymInvitationRequest request, 
        CancellationToken cancellationToken)
    {
        var user = await _identityProvider.GetCurrentAsync(cancellationToken);

        var userEmail = user.RequiredUserEmail();

        var gymInvitation =
            await _context
            .UserGymInvitations
            .Where(e => e.Id == request.InvitationId)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new UnauthorizedCoreException();

        if (userEmail.Equals(gymInvitation.UserEmail, StringComparison.OrdinalIgnoreCase))
        {
            throw new UnauthorizedCoreException();
        }

        if (DateTime.UtcNow > gymInvitation.ExpiresAt)
            throw new UnauthorizedCoreException();

        var userData =
            await _context
            .Users
            .Where(e => e.UserName == userEmail)
            .Select(e => new
            {
                e.Id, e.UserName
            })
            .SingleOrDefaultAsync(cancellationToken);

        if (userData is null)
        {
            return new(AcceptGymInvitationStatusResponse.EmailIsNotRegistered);
        }

        var alreadyContainsRoleFromUser =
            await _context
            .UserTrainingRoles
            .Where(e => e.UserId == userData.Id && e.GymGroupId == gymInvitation.GymId)
            .AnyAsync(cancellationToken);

        if (alreadyContainsRoleFromUser)
        {
            _logger.LogInformation("Role already setted to user {0} and gym {1}.", userData.Id, gymInvitation.GymId);
            return new(AcceptGymInvitationStatusResponse.Accepted);
        }

        var normalizedRoleName = gymInvitation.RoleName.ToUpperInvariant().Trim();

        var roleData = 
            await _context
            .Roles
            .Where(e => e.NormalizedName == normalizedRoleName)
            .Select(e => new
            {
                e.Id
            })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new CommonCoreException("Role not found.");

        await using var transaction
            = await _context.Database.BeginTransactionAsync(cancellationToken);

        await _context
            .UserTrainingRoles
            .AddAsync(new Model.Identity.UserRoleTrainingModel
            {
                Id = Guid.Empty,
                GymGroupId = gymInvitation.GymId,
                RoleId = roleData.Id,
                UserId = userData.Id,
            });

        gymInvitation.Accepted = true;

        await _context.SaveChangesAsync();

        await transaction.CommitAsync();

        return new(AcceptGymInvitationStatusResponse.Accepted);
    }
}