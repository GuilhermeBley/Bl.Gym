
using Bl.Gym.TrainingApi.Application.Providers;
using Bl.Gym.TrainingApi.Application.Repositories;
using Bl.Gym.TrainingApi.Application.Services;
using Bl.Gym.TrainingApi.Domain.Entities.Identity;
using System;

namespace Bl.Gym.TrainingApi.Application.Commands.Identity.SendGymInvitationToUser;

public class SendGymInvitationToUserHandler
    : IRequestHandler<SendGymInvitationToUserRequest, SendGymInvitationToUserResponse>
{
    private readonly IIdentityProvider _identityProvider;
    private readonly TrainingContext _context;
    private readonly IEmailService _emailService;

    public async Task<SendGymInvitationToUserResponse> Handle(
        SendGymInvitationToUserRequest request, 
        CancellationToken cancellationToken)
    {
        var currentUser =
            await _identityProvider.GetCurrentAsync();

        var userId = currentUser.RequiredUserId();

        currentUser.EnsureGymId(request.GymId);

        UserGymInvitation.Create(
            id: Guid.Empty,
            invitedByUserId: userId,
            userEmail: request.Email,
            gymId: request.GymId,
            expiresAt: DateTime.UtcNow.AddDays(1),
            gymGroupRole: Role.Admin.Name,
            createdAt: DateTime.UtcNow);
    }
}
