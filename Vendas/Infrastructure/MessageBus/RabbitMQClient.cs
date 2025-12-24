

using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Infrastructure.MessageBus
{
    public class RabbitMQClient(IConfiguration config) : IMessageBusClient, IAsyncDisposable
    {
        private IConnection? _connection;
        private IChannel? _channel;
        private ConnectionFactory? _factory;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _factory = new ConnectionFactory()
            {
                HostName = config["Rabbit:Host"] ?? "localhost"
            };

            _connection = await _factory.CreateConnectionAsync(cancellationToken);
            _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);
        }

        public async Task Subscribe<T>(string exchange, string queue, string routingKey, Func<T, Task> handleMessage)
        {
            if (_channel is null)
                throw new InvalidOperationException("RabbitMQ channel não inicializado. ");

            // Garantir que exchange exista
            await _channel.ExchangeDeclareAsync(exchange, "direct", durable: true, autoDelete: false);

            // Criar queue específica para o serviço de Vendas
            await _channel.QueueDeclareAsync(queue, durable: true, exclusive: false, autoDelete: false);

            // Bind na routingKey do evento
            await _channel.QueueBindAsync(queue, exchange, routingKey);

            // Configurar consumer
            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                try
                {
                    var messageBody = Encoding.UTF8.GetString(ea.Body.ToArray());
                    var messageObj = JsonSerializer.Deserialize<T>(messageBody);

                    if (messageObj == null)
                    {
                        Console.WriteLine("Mensagem inválida recebida. Não foi possível desserializar.");
                        await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
                        return;
                    }

                    // Executa lógica do handler passado
                    await handleMessage(messageObj);

                    // Confirma processamento da mensagem
                    await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao processar mensagem: {ex.Message}");
                    await _channel.BasicNackAsync(ea.DeliveryTag, false, requeue: true);
                }
            };

            // Consumir mensagens da fila
            await _channel.BasicConsumeAsync(queue, autoAck: false, consumer);
        }

        public async Task Publish(object message, string routingKey, string exchange, CancellationToken cancellationToken = default)
        {
            if (_channel is null)
                throw new InvalidOperationException("RabbitMQ channel não inicializado. ");

            // Declaração do exchange
            await _channel.ExchangeDeclareAsync(
                exchange: exchange,
                type: ExchangeType.Direct,
                durable: true,
                autoDelete: false,
                cancellationToken: cancellationToken
            );

            var body = JsonSerializer.SerializeToUtf8Bytes(message);

            var properties = new BasicProperties
            {
                DeliveryMode = DeliveryModes.Persistent,
                ContentType = "application/json",
                MessageId = Guid.NewGuid().ToString(),
                Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds()),
                Type = message.GetType().Name
            };

            await _channel.BasicPublishAsync(
                exchange: exchange,
                routingKey: routingKey,
                mandatory: false,
                basicProperties: properties,
                body: body,
                cancellationToken: cancellationToken
            );
        }

        // Não usei GC.SuppressFinalize, porque não há finalizador na classe
        public async ValueTask DisposeAsync()
        {
            if (_channel is not null)
                await _channel.CloseAsync();
            if (_connection is not null)
                await _connection.CloseAsync();
        }

    }
}
