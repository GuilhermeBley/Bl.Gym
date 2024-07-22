namespace Bl.Gym.EventBus;

public interface IEventBus
{
    Task SendMessageAsync<TEvent>(
        TEvent @event, 
        CancellationToken cancellationToken = default) 
        where TEvent : class, IIntegrationEvent;
}