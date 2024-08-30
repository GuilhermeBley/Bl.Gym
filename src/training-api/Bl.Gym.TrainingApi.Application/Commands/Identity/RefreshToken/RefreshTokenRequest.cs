namespace Bl.Gym.TrainingApi.Application.Commands.Identity.RefreshToken;

public record RefreshTokenRequest(
    Guid UserId,
    string RefreshToken)
    : IRequest<RefreshTokenResponse>;
