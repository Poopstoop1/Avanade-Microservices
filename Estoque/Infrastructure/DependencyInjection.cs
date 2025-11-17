
using Application.Interfaces;
using Domain.IRepository;
using Infrastructure.DB;
using Infrastructure.MessageBus;
using Infrastructure.MessageBus.Consumers;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Infrastructure
{
    public static class DependencyInjection
    {
         public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddDbContext<EstoqueDBContext>(options =>
             options.UseSqlServer(configuration.GetConnectionString("EstoqueConnection")));
            return services;
        }

        public static IServiceCollection AddMessageBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IMessageBusClient>(sp =>
                 new RabbitMQClient(configuration)
             );

            services.AddSingleton<IPedidoConfirmConsumer, PedidoConfirmadoConsumer>();
            return services;
        }
    }
}