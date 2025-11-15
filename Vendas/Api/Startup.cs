using Infrastructure;
using Microsoft.OpenApi.Models;
using Application;

namespace Vendas
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
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Serviço Pedido",
                    Version = "v1",
                    Description = "API para gerenciamento de Vendas e Pedidos"
                });
            });
            services.AddInfrastructure(Configuration);
            services.AddMessageBus(Configuration);
            services.AddApplication();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Serviço Estoque v1");
                    c.DocumentTitle = "Serviço Vendas";
                });
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
