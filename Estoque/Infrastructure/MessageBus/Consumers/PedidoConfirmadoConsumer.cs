using Application.Interfaces;
using Domain.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.MessageBus.Consumers
{
    public class PedidoConfirmadoConsumer : IPedidoConfirmConsumer
    {
        private readonly IMessageBusClient _messageBus;

        private readonly IServiceProvider _serviceProvider;

        public PedidoConfirmadoConsumer(IMessageBusClient rabbit, IServiceProvider serviceProvider)
        {
            _messageBus = rabbit;
            _serviceProvider = serviceProvider;
        }

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
