

namespace Infrastructure.MessageBus
{
    public interface IMessageBusClient
    {
        Task Publish(object message, string routingKey, string exchange, CancellationToken cancellationToken);
    }
}
