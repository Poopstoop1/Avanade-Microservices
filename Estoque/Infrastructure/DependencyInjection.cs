using System;
using Application.Interfaces;
using Application.Service;
using Domain.IRepository;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
         public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<IProdutoService, ProdutoService>();
            return services;
        }
    }
}