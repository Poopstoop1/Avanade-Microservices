

using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
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

            _connection = factory.CreateConnectionAsync("Estoque-Service-Consumer").GetAwaiter().GetResult();
            _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();
        }

        public Task Subscribe<T>(string exchange, string queue, string routingKey, Func<T, Task> handleMessage)
        {

            // Garantir que exchange exista
            _channel.ExchangeDeclareAsync(exchange, "fanout", durable: true, autoDelete: false).GetAwaiter().GetResult();

            // Criar queue específica para o serviço de Estoque
            _channel.QueueDeclareAsync(queue, durable: true, exclusive: false, autoDelete: false).GetAwaiter().GetResult();

            // Bind na routingKey do evento
            _channel.QueueBindAsync(queue, exchange, routingKey).GetAwaiter().GetResult();

            // Configurar consumer
            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async (model, ea) =>
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
            };

            // Consumir mensagens da fila
            _channel.BasicConsumeAsync(queue, autoAck: false, consumer).GetAwaiter().GetResult();
            return Task.CompletedTask;
        }
    }
}
