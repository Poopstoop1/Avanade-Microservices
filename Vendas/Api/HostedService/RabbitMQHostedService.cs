
using Application.Interfaces;


namespace Vendas.HostedService
{
    public class RabbitmqHostedService(IEnumerable<IConsumer> consumers) : BackgroundService
    {
        private readonly IEnumerable<IConsumer> _consumers = consumers;

        

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var tasks = _consumers
                .Select(c => c.StartAsync(stoppingToken));

            await Task.WhenAll(tasks);
        }
    }
}
   

