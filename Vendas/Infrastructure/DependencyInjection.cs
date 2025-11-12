using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
