using Api.Entities;
using Api.Infrastructure.Data;
using Api.Infrastructure.Repositories;
using Api.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;


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
            var jwtSettings = Configuration.GetSection("JwtSettings");

            var secret = jwtSettings["Secret"]
                ?? throw new InvalidOperationException("JWT Secret não configurado em appsettings.json");

            var issuer = jwtSettings["Issuer"]
                ?? throw new InvalidOperationException("JWT Issuer não configurado em appsettings.json");

            var audience = jwtSettings["Audience"]
                ?? throw new InvalidOperationException("JWT Audience não configurado em appsettings.json");

            var key = Encoding.UTF8.GetBytes(secret);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer("JwtBearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });
            services.AddEndpointsApiExplorer();
            services.AddReverseProxy()
                .LoadFromConfig(Configuration.GetSection("ReverseProxy"));
             services.AddDbContext<GatewayDbContext>(options =>
             options.UseSqlServer(Configuration.GetConnectionString("GatewayConnection")));

            services.AddSingleton<IPasswordHasher<Usuario>, PasswordHasher<Usuario>> ();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();


        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }



            app.UseEndpoints(endpoints =>
            {
                endpoints.MapReverseProxy();
            });
        }
    }
}

