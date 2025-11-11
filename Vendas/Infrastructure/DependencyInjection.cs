using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vendas.Infrastructure.DB;

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
    }
}
