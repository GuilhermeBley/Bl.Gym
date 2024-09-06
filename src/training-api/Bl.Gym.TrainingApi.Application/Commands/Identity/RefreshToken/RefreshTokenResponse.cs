using System.Security.Claims;

namespace Bl.Gym.TrainingApi.Application.Commands.Identity.RefreshToken;

public record RefreshTokenResponse(
    string Username,
    string Email,
    string RefreshToken,
    IReadOnlyList<Claim> Claims);
