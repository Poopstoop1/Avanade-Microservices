using Infrastructure.Data;
using Infrastructure.MessageBus;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Application.Interfaces;
using Infrastructure.MessageBus.Consumers;
using Domain.IRepository;
using infrastructure.CacheStorage;

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


        public static IServiceCollection AddMessageBus(this IServiceCollection services)
        {
            services.AddSingleton<RabbitMQClient>();

            services.AddSingleton<IMessageBusClient>(sp =>
                sp.GetRequiredService<RabbitMQClient>()
            );

            services.AddSingleton<IConsumer,EstoqueRejeitadoConsumer>();

            return services;
        }

        public static IServiceCollection AddRedisCache(this IServiceCollection services)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "localhost:6379";
                options.InstanceName = "VendasCache";
            });

            services.AddTransient<ICacheService, CacheService>();
            
            return services;
        }
    }
}
