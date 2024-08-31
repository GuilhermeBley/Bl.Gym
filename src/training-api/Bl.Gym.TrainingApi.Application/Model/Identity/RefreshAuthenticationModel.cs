using Bl.Gym.TrainingApi.Domain.Entities.Identity;

namespace Bl.Gym.TrainingApi.Application.Model.Identity;

public class RefreshAuthenticationModel
{
    public Guid UserId { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime RefreshTokenExpiration { get; set; }
    public Guid ConcurrencyStamp { get; set; }
    public DateTime UpdatedAt { get; set; }

    public static RefreshAuthenticationModel MapFromEntity(
        RefreshAuthentication entity)
    {
        return new()
        {
            RefreshToken = entity.RefreshToken,
            RefreshTokenExpiration = entity.RefreshTokenExpiration,
            UpdatedAt = entity.UpdatedAt,
            UserId = entity.UserId,
            ConcurrencyStamp = Guid.NewGuid(),
        };
    }
}
