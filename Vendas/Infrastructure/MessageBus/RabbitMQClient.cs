

using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Infrastructure.MessageBus
{
    public class RabbitMQClient : IMessageBusClient
    {
        private readonly IConnection _connection;
        private readonly IChannel _channel;

        private RabbitMQClient(IConnection connection, IChannel channel)
        {
            _connection = connection;
            _channel = channel;
        }

        public static async Task<RabbitMQClient> CreateAsync(IConfiguration config)
        {
            var factory = new ConnectionFactory()
            {
                HostName = config["Rabbit:Host"] ?? "localhost"
            };

            var connection = await factory.CreateConnectionAsync("Pedido-Service-Producer");
            var channel = await connection.CreateChannelAsync();

            return new RabbitMQClient(connection, channel);
        }

        public async Task Publish(object message, string routingKey, string exchange, CancellationToken cancellationToken = default)
        {
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
            var properties = new BasicProperties();

            await _channel.BasicPublishAsync<BasicProperties>(
                exchange: exchange,
                routingKey: routingKey,
                mandatory: false,
                basicProperties: properties,
                body: body,
                cancellationToken: cancellationToken
            );
        }
    }
}
