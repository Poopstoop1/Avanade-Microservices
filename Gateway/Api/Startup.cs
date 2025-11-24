using Api.Entities;
using Api.Infrastructure.Data;
using Api.Infrastructure.Repositories;
using Api.Interfaces;
using Api.Middleware;
using Api.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;
using Yarp.ReverseProxy.Transforms;

namespace Api
{
    public class Startup
    {

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var configurationYarp = Configuration.GetSection("ReverseProxy");
            services.AddEndpointsApiExplorer();
            services.AddReverseProxy()
            .LoadFromConfig(configurationYarp);
            services.AddDbContext<GatewayDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("GatewayConnection")));

            services.AddSingleton<IPasswordHasher<Usuario>, PasswordHasher<Usuario>>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddSingleton<JwtService>();
            services.AddTransient<AuthService>();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("gateway", new() { Title = "API Gateway", Version = "v1"  });
              

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Insira o token JWT desta forma: Bearer {seu token}",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                         new OpenApiSecurityScheme
                         {
                             Reference = new OpenApiReference
                             {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                             }
                         },
                         Array.Empty<string>()
                    }
                });     
            });

            services.AddControllers();
            services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                var jwtSettings = Configuration.GetSection("JwtSettings");

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

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseMiddleware<SwaggerPrefixMiddleware>();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/gateway/swagger.json", "Gateway");

                    c.SwaggerEndpoint(
                        "/vendas/swagger/v1/swagger.json",
                        "Vendas"
                    );

                    c.SwaggerEndpoint(
                        "/estoque/swagger/v1/swagger.json",
                        "Estoque"
                    );
                });
            }
          
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapReverseProxy();
                endpoints.MapControllers();
            });
        }
    }
}

