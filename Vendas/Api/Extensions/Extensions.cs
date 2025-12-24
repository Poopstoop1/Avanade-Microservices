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

        public static IServiceCollection AddAuthenticationJwt(
        this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = jwtSettings["Issuer"],
                        ValidAudience = jwtSettings["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtSettings["Secret"]!)
                        ),
                        RoleClaimType = ClaimTypes.Role
                    };
                });

            return services;
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
