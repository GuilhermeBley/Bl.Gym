namespace Bl.Gym.TrainingApi.Application.Commands.Identity.Login;

public record LoginRequest(
    string Login,
    string Password)
    : IRequest<LoginResponse>;
