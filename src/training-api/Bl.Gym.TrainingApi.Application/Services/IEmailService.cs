namespace Bl.Gym.TrainingApi.Application.Services;

public interface IEmailService
{
    Task SendGymInvitationEmailAsync(
        string email,
        CancellationToken cancellationToken = default);
}
