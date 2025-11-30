using Application.Interfaces;
using Domain.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.MessageBus.Consumers
{
    public class EstoqueRejeitadoConsumer : IConsumer
    {
        private readonly IMessageBusClient _messageBus;

        private readonly IServiceProvider _serviceProvider;

        public EstoqueRejeitadoConsumer(IMessageBusClient messageBus, IServiceProvider serviceProvider)
        {
            _messageBus = messageBus;
            _serviceProvider = serviceProvider;
        }


        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _messageBus.Subscribe<EstoqueRejeitadoEvent>(
                exchange: "estoque.exchange",
                queue: "pedido_cancelado",
                routingKey: "estoque-indisponivel",
                handleMessage: async (estoqueRejeitadoEvent) =>
                {
                    try
                    {
                        using var scope = _serviceProvider.CreateScope();

                        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                        await mediator.Send(estoqueRejeitadoEvent, cancellationToken);
                        Console.WriteLine("Pedido Cancelado Processando!");

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
