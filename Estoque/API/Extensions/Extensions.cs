using API.HostedService;
using Infrastructure;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class Extensions
    {
        public static void ApplyMigrations(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<EstoqueDBContext>();
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
