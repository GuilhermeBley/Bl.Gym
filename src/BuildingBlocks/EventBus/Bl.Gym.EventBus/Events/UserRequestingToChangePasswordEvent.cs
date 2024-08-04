
namespace Bl.Gym.EventBus.Events;

/// <summary>
/// Represents an user password change request.
/// </summary>
public class UserRequestingToChangePasswordEvent
    : IIntegrationEvent
{
    public Guid EventId { get; set; } = Guid.NewGuid();
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public Guid UserId { get; set; }
}
