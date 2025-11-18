
using Application.Interfaces;


namespace API.HostedService
{
    public class RabbitmqHostedService : BackgroundService
    {
        private readonly IPedidoConfirmConsumer _pedidoConsumer;

        public RabbitmqHostedService(IPedidoConfirmConsumer pedidoConsumer)
        {
            _pedidoConsumer = pedidoConsumer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _pedidoConsumer.StartAsync(stoppingToken);
        }
    }
}
   

