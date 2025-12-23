using Application.Interfaces;
using Infrastructure.MessageBus;


namespace API.HostedService
{
    public class RabbitmqHostedService(RabbitMQClient bus,IEnumerable<IConsumer> consumers) : BackgroundService
    {
        private readonly RabbitMQClient _bus = bus;
        private readonly IEnumerable<IConsumer> _consumers = consumers;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // 🔥 1. Inicializa conexão e channel UMA VEZ
            await _bus.StartAsync(stoppingToken);

            // 🔥 2. Inicia todos os consumers
            var tasks = _consumers
                .Select(c => c.StartAsync(stoppingToken));

            await Task.WhenAll(tasks);

            // 🔥 3. Mantém o HostedService vivo
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await _bus.DisposeAsync();
            await base.StopAsync(cancellationToken);
        }
    }
}
   

