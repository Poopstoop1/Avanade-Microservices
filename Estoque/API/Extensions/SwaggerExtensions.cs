using Microsoft.OpenApi.Models;


namespace API.Extensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(c =>
            {
                c.CustomSchemaIds(id => id.FullName!.Replace('+', '-'));

                c.AddServer(new OpenApiServer
                {
                    Url = "http://localhost:8000/estoque"
                });

                c.AddSecurityDefinition("Keycloak", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri(configuration["Keycloak:AuthorizationUrl"]!),
                            TokenUrl = new Uri(configuration["Keycloak:TokenUrl"]!),
                            Scopes = new Dictionary<string, string>
                            {
                                {"openid", "openid"},
                                {"profile", "profile"}
                            }
                        }
                    }
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                           Reference = new OpenApiReference
                           {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Keycloak"
                           }
                        },
                            Array.Empty<string>()
                    }
                });

            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerDocumentation(
            this IApplicationBuilder app,
            IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/estoque/swagger/v1/swagger.json", "Serviço Estoque v1");
                    c.DocumentTitle = "Serviço Estoque";

                    c.OAuthClientId("api-backend-estoque-vendas");
                    c.OAuthUsePkce();
                    c.OAuthScopes("openid", "profile");
                });
            }

            return app;
        }

    }



}
