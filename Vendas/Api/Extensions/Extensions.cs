using Infrastructure;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Vendas.HostedService;

namespace Vendas.Extensions
{
    public static class Extensions
    {
        public static void ApplyMigrations(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VendasDBContext>();
            db.Database.Migrate();
        }

        public static IServiceCollection AddMessaging(
        this IServiceCollection services)
        {
            services.AddMessageBus();
            services.AddHostedService<RabbitmqHostedService>();

            return services;
        }
    }
}
