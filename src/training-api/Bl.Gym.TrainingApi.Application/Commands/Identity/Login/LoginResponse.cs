namespace Bl.Gym.TrainingApi.Application.Commands.Identity.Login;

public record LoginResponse(
    string Username,
    string Email,
    IEnumerable<(string ClaimType, string ClaimValue)> Claims,
    string Token);
