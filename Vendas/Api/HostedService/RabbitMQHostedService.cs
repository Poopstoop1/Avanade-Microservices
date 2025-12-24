using Application.Interfaces;
using Infrastructure.MessageBus;

namespace Vendas.HostedService
{
    public class RabbitmqHostedService(RabbitMQClient bus, IEnumerable<IConsumer> consumers) : BackgroundService
    {
        private readonly RabbitMQClient _bus = bus;
        private readonly IEnumerable<IConsumer> _consumers = consumers;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _bus.StartAsync(stoppingToken);

            var tasks = _consumers
                .Select(c => c.StartAsync(stoppingToken));

            await Task.WhenAll(tasks);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await _bus.DisposeAsync();
            await base.StopAsync(cancellationToken);
        }
    }
}
   

