namespace Bl.Gym.EventBus;

public interface IIntegrationEvent
{
    Guid EventId { get; }
    DateTimeOffset CreatedAt { get; }
}
