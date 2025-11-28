


namespace Application.Interfaces
{
    public interface IMessageBusClient 
    {
        Task Subscribe<T>(string exchange, string queue, string routingKey, Func<T,Task> handleMessage);
        Task Publish(object message, string routingKey, string exchange, CancellationToken cancellationToken);
    }
}
