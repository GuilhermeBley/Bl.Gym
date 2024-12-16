namespace Bl.Gym.TrainingApi.Application.Services;

public interface IEmailService
{
    Task SendGymInvitationEmailAsync(
        Domain.Entities.Identity.UserGymInvitation invite,
        CancellationToken cancellationToken = default);
}
