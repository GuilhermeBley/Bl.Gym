namespace Bl.Gym.TrainingApi.Application.Commands.Identity.RequestToChangePassword;

public record RequestToChangePasswordRequest(
    string Email
) : IRequest<RequestToChangePasswordResponse>;
