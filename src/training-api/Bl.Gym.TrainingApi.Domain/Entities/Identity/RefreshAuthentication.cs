﻿namespace Bl.Gym.TrainingApi.Domain.Entities.Identity;

public class RefreshAuthentication
    : Entity
{
    public Guid UserId { get; private set; }
    public string RefreshToken { get; private set; } = string.Empty;
    public DateTime RefreshTokenExpiration { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public Result<RefreshAuthentication> Create(
        Guid userId,
        string newRefreshToken,
        TimeSpan expiresIn)
        => Create(
            userId, 
            newRefreshToken, 
            DateTime.UtcNow.Add(expiresIn),
            DateTime.UtcNow);

    public Result<RefreshAuthentication> Create(
        Guid userId,
        string refreshToken,
        DateTime refreshTokenExpiration,
        DateTime updatedAt)
    {
        ResultBuilder<RefreshAuthentication> builder = new();

        builder.AddIf(refreshTokenExpiration < DateTime.UtcNow, CoreExceptionCode.InvalidExpirationTokenDate);

        builder.AddIf(refreshToken.Length < 10, CoreExceptionCode.InvalidRefreshTokenLength);

        return builder.CreateResult(() =>
        {
            return new()
            {
                RefreshToken = refreshToken,
                RefreshTokenExpiration = refreshTokenExpiration,
                UpdatedAt = updatedAt,
                UserId = userId
            };
        });
    }
}
