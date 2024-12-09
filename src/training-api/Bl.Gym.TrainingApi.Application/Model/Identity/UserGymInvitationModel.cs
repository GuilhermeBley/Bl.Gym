using Bl.Gym.TrainingApi.Domain.Entities.Identity;

namespace Bl.Gym.TrainingApi.Application.Model.Identity;

public class UserGymInvitationModel
{
    public Guid Id { get; set; }
    public Guid InvitedByUserId { get; set; }
    public string UserEmail { get; set; } = string.Empty;
    public Guid GymId { get; set; }
    public DateTime ExpiresAt { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public static UserGymInvitationModel MapFromEntity(
        UserGymInvitation entity)
    {
        return new()
        {
            ExpiresAt = entity.ExpiresAt,
            CreatedAt = entity.CreatedAt,
            RoleName = entity.RoleName,
            GymId = entity.GymId,
            Id = entity.Id,
            InvitedByUserId = entity.InvitedByUserId,
            UserEmail = entity.UserEmail,
        };
    }
}
