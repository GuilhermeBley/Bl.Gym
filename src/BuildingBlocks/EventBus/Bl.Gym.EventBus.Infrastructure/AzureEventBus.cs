using Azure.Messaging.ServiceBus;

namespace Bl.Gym.EventBus.Infrastructure;

public class AzureEventBus
    : IEventBus, IAsyncDisposable, IDisposable
{
    private readonly ServiceBusClient _client;

    public AzureEventBus(string connectionsString)
    {
        _client = new ServiceBusClient(connectionsString);
    }

    public void Dispose()
    {
        _client.DisposeAsync().GetAwaiter().GetResult();
    }

    public ValueTask DisposeAsync()
    {
        return _client.DisposeAsync();
    }

    async Task IEventBus.SendMessageAsync<TEvent>(
        TEvent @event, 
        CancellationToken cancellationToken)
    {
        var jsonMessage = System.Text.Json.JsonSerializer.Serialize(@event);

        var message = new ServiceBusMessage(jsonMessage)
        {
            MessageId = Guid.NewGuid().ToString(),
        };

        await using var sender = _client.CreateSender(
            GetQueueOrTopicNameByEvent(@event));

        await sender.SendMessageAsync(message, cancellationToken);
    }

    private static string GetQueueOrTopicNameByEvent<TEvent>(
        TEvent @event)
        => nameof(TEvent);
}
