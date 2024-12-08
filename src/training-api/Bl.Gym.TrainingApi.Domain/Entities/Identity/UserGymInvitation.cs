namespace Bl.Gym.TrainingApi.Domain.Entities.Identity;

/// <summary>
/// Class represents an entity whose responsibility is to store all user invitations to gyms.
/// </summary>
public class UserGymInvitation
    : Entity
{
    public Guid Id { get; private set; }
    public Guid InvitedByUserId { get; private set; }
    public string UserEmail { get; private set; } = string.Empty;
    public Guid GymId { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public string RoleName { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    private UserGymInvitation() { }

    public override bool Equals(object? obj)
    {
        return obj is UserGymInvitation invitation &&
               base.Equals(obj) &&
               EntityId.Equals(invitation.EntityId) &&
               Id.Equals(invitation.Id) &&
               InvitedByUserId.Equals(invitation.InvitedByUserId) &&
               UserEmail == invitation.UserEmail &&
               GymId.Equals(invitation.GymId) &&
               ExpiresAt == invitation.ExpiresAt &&
               RoleName == invitation.RoleName &&
               CreatedAt == invitation.CreatedAt;
    }

    public override int GetHashCode()
    {
        HashCode hash = new HashCode();
        hash.Add(base.GetHashCode());
        hash.Add(EntityId);
        hash.Add(Id);
        hash.Add(InvitedByUserId);
        hash.Add(UserEmail);
        hash.Add(GymId);
        hash.Add(ExpiresAt);
        hash.Add(RoleName);
        hash.Add(CreatedAt);
        return hash.ToHashCode();
    }

    public bool IsExpired(IDateTimeProvider? dateTimeProvider = null)
    {
        dateTimeProvider ??= new DateTimeProvider();

        return ExpiresAt > dateTimeProvider.UtcNow;
    }

    public static Result<UserGymInvitation> Create(
        Guid id,
        Guid invitedByUserId,
        string userEmail,
        Guid gymId,
        DateTime expiresAt,
        string gymGroupRole,
        DateTime createdAt)
    {
        ResultBuilder<UserGymInvitation> builder = new();

        builder.AddIf(!Role.IsRoleRegistered(gymGroupRole), CoreExceptionCode.InvalidRoleGroup);

        builder.AddIf(expiresAt < createdAt, CoreExceptionCode.InvalidExpiration);

        userEmail = userEmail?.Trim().ToLowerInvariant() ?? string.Empty;

        builder.AddIf(Validations.EmailValidation.IsInvalidEmail(userEmail), CoreExceptionCode.InvalidEmail);

        return builder.CreateResult(() =>
            new UserGymInvitation()
            {
                CreatedAt = createdAt,
                ExpiresAt = expiresAt,
                GymId = gymId,
                RoleName = userEmail,
                Id = id,
                InvitedByUserId = invitedByUserId,
                UserEmail = userEmail,

            });
    }
}
