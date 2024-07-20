namespace Bl.Gym.TrainingApi.Application.Commands.Identity.ChangePassword;

public record ChangePasswordRequest(
    string email,
    string newPassword)
    : IRequest<ChangePasswordResponse>;
