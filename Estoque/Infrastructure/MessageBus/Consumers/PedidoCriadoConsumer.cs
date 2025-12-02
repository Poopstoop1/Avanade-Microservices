
using Application.Interfaces;
using Domain.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
namespace Infrastructure.MessageBus.Consumers
{
    public class PedidoCriadoConsumer(IMessageBusClient messageBus, IServiceProvider serviceProvider) : IConsumer
    {
        private readonly IMessageBusClient _messageBus = messageBus;
        
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _messageBus.Subscribe<PedidoCriadoEvent>(
                exchange: "pedido.exchange",
                queue: "estoque_pedido_reservado",
                routingKey: "pedido-criado",
                handleMessage: async (pedidoCriadoEvent) =>
                {
                    try
                    {
                        using var scope = _serviceProvider.CreateScope();

                        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                        await mediator.Send(pedidoCriadoEvent, cancellationToken);
                        Console.WriteLine("Pedido criado processado!");

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"ERRO no handler: {ex.Message}");
                        throw;
                    }
                }
            );
        }

    }
}
