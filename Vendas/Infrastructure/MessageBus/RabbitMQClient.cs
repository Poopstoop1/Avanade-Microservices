

using Application.Interfaces;
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

        public RabbitMQClient(IConfiguration config)
        {
            var factory = new ConnectionFactory()
            {
                HostName = config["Rabbit:Host"] ?? "localhost"
            };

            _connection = factory.CreateConnectionAsync("Pedido-Service-Producer").GetAwaiter().GetResult();
            _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();
        }

        public async Task Publish(object message, string routingKey, string exchange, CancellationToken cancellationToken = default)
        {
            // Declaração do exchange
            await _channel.ExchangeDeclareAsync(
                exchange: exchange,
                type: ExchangeType.Direct,
                durable: true,
                autoDelete: false,
                cancellationToken: cancellationToken
            );

            // Declaração da fila
            await _channel.QueueDeclareAsync(
                 queue: routingKey,
                 durable: false,
                 exclusive: false,
                 autoDelete: false,
                 arguments: null,
                 cancellationToken: cancellationToken
            );


            // Vincular Fila ao Exchange
            await _channel.QueueBindAsync(
                queue: routingKey,
                exchange: exchange,
                routingKey: routingKey,
                cancellationToken: cancellationToken
            );

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
