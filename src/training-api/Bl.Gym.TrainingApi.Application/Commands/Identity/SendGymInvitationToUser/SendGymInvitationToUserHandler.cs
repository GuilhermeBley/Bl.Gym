
using Bl.Gym.TrainingApi.Application.Model.Identity;
using Bl.Gym.TrainingApi.Application.Providers;
using Bl.Gym.TrainingApi.Application.Repositories;
using Bl.Gym.TrainingApi.Application.Services;
using Bl.Gym.TrainingApi.Domain.Entities.Identity;

namespace Bl.Gym.TrainingApi.Application.Commands.Identity.SendGymInvitationToUser;

public class SendGymInvitationToUserHandler
    : IRequestHandler<SendGymInvitationToUserRequest, SendGymInvitationToUserResponse>
{
    private readonly IIdentityProvider _identityProvider;
    private readonly TrainingContext _context;
    private readonly IEmailService _emailService;

    public SendGymInvitationToUserHandler(
        IIdentityProvider identityProvider, 
        TrainingContext context, 
        IEmailService emailService)
    {
        _identityProvider = identityProvider;
        _context = context;
        _emailService = emailService;
    }

    public async Task<SendGymInvitationToUserResponse> Handle(
        SendGymInvitationToUserRequest request, 
        CancellationToken cancellationToken)
    {
        var currentUser =
            await _identityProvider.GetCurrentAsync();

        var userId = currentUser.RequiredUserId();

        currentUser.EnsureGymId(request.GymId);

        var invitation = UserGymInvitation.Create(
            id: Guid.Empty,
            invitedByUserId: userId,
            userEmail: request.Email,
            gymId: request.GymId,
            expiresAt: DateTime.UtcNow.AddDays(1),
            gymGroupRole: Role.Admin.Name,
            createdAt: DateTime.UtcNow)
            .RequiredResult;

        var model = UserGymInvitationModel.MapFromEntity(invitation);

        await using var transaction 
            = await _context.Database.BeginTransactionAsync();

        await _context.AddAsync(model);

        await _context.SaveChangesAsync();

        await _emailService.SendGymInvitationEmailAsync(
            invitation,
            cancellationToken);

        await transaction.CommitAsync();

        return new();
    }
}
