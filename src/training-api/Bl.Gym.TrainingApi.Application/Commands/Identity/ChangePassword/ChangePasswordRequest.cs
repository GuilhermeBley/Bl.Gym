namespace Bl.Gym.TrainingApi.Application.Commands.Identity.ChangePassword;

public record ChangePasswordRequest(
    string Email,
    string NewPassword)
    : IRequest<ChangePasswordResponse>;
