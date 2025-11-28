
using Application.Interfaces;


namespace API.HostedService
{
    public class RabbitmqHostedService : BackgroundService
    {
        private readonly IConsumer _pedidoConsumer;

        public RabbitmqHostedService(IConsumer pedidoConsumer)
        {
            _pedidoConsumer = pedidoConsumer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _pedidoConsumer.StartAsync(stoppingToken);
        }
    }
}
   

