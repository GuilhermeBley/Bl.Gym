using Bl.Gym.TrainingApi.Domain.Entities.Identity;

namespace Bl.Gym.TrainingApi.Application.Model.Identity;

public class UserModel
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string NormalizedUserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string NormalizedEmail { get; set; } = string.Empty;
    public bool EmailConfirmed { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    public Guid? SecurityStamp { get; set; }
    public Guid? ConcurrencyStamp { get; set; }
    public string? PhoneNumber { get; set; }
    public bool PhoneNumberConfirmed { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public DateTimeOffset? LockoutEnd { get; set; }
    public bool LockoutEnabled { get; set; }
    public int AccessFailedCount { get; set; }

    public static UserModel MapFromEntity(
        User entity)
    {
        return new()
        {
            AccessFailedCount = entity.AccessFailedCount,
            ConcurrencyStamp = entity.ConcurrencyStamp,
            Email = entity.Email,
            EmailConfirmed = entity.EmailConfirmed,
            PasswordHash = entity.PasswordHash,
            PhoneNumber = entity.PhoneNumber,
            TwoFactorEnabled = entity.TwoFactorEnabled,
            FirstName = entity.FirstName,
            Id = entity.Id,
            LastName = entity.LastName,
            LockoutEnabled = entity.LockoutEnabled,
            LockoutEnd = entity.LockoutEnd,
            NormalizedEmail = entity.NormalizedEmail,
            NormalizedUserName = entity.NormalizedUserName,
            PhoneNumberConfirmed = entity.PhoneNumberConfirmed,
            SecurityStamp = entity.SecurityStamp,
            UserName = entity.UserName,
        };
    }
}
