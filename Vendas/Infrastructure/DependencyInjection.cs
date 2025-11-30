using Infrastructure.Data;
using Infrastructure.MessageBus;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Application.Interfaces;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IPedidoRepository, PedidoRepository>();

            services.AddDbContext<VendasDBContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("VendasConnection"));
            });

            return services;
        }


        public static IServiceCollection AddMessageBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IMessageBusClient>(sp =>
                 new RabbitMQClient(configuration)
             );
            return services;
        }
    }
}
