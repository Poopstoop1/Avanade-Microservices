
using Domain.IRepository;
using Infrastructure.DB;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

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
    }
}