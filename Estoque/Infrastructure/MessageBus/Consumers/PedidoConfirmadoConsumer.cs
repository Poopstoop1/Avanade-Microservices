using Application.Interfaces;
using Domain.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.MessageBus.Consumers
{
    public class PedidoConfirmadoConsumer(IMessageBusClient rabbit, IServiceProvider serviceProvider) : IConsumer
    {
        private readonly IMessageBusClient _messageBus = rabbit;

        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _messageBus.Subscribe<PedidoConfirmadoEvent>(
                exchange: "pedido_exchange",
                queue: "estoque_pedido_confirmado",
                routingKey: "pedido.confirmado",
                handleMessage: async (pedidoConfirmadoEvent) =>
                {
                    try
                    {
                        using var scope = _serviceProvider.CreateScope();

                        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                        await mediator.Send(pedidoConfirmadoEvent, cancellationToken);

                        Console.WriteLine("Pedido confirmado processado!");
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
