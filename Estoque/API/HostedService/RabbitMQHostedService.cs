
using Application.Interfaces;


namespace API.HostedService
{
    public class RabbitmqHostedService : BackgroundService
    {
        private readonly IEnumerable<IConsumer> _consumers;

        public RabbitmqHostedService(IEnumerable<IConsumer> consumers)
        {
            _consumers = consumers;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var tasks = _consumers
                .Select(c => c.StartAsync(stoppingToken));

            await Task.WhenAll(tasks);
        }
    }
}
   

