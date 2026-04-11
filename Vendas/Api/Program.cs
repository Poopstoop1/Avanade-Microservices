using Application;
using Infrastructure;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Vendas.Extensions;



/*
* ASP.NET Core Web API application setup micro serviço Vendas.
* Author: Paulo Daniel
* Date: Novembro de 2025
*/


var builder = WebApplication.CreateBuilder(args);
#region services
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocumentation(builder.Configuration);
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "http://localhost:8080/realms/estoque-vendas";
        options.RequireHttpsMetadata = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false
        };

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                var identity = context.Principal?.Identity as ClaimsIdentity;

                var realmAccess = context.Principal?.FindFirst("realm_access")?.Value;

                if (realmAccess != null)
                {
                    var json = System.Text.Json.JsonDocument.Parse(realmAccess);

                    if (json.RootElement.TryGetProperty("roles", out var roles))
                    {
                        foreach (var role in roles.EnumerateArray())
                        {
                            var roleValue = role.GetString();
                            if (!string.IsNullOrEmpty(roleValue))
                            {
                                identity?.AddClaim(new Claim(ClaimTypes.Role, roleValue));
                            }
                        }
                    }
                }

                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddMessaging();
builder.Services.AddRedisCache();
#endregion



#region AppSettings
var app = builder.Build();
app.UseRouting();
app.ApplyMigrations();
app.UseSwaggerDocumentation(app.Environment);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
#endregion

#region TesteDatabaseConnection
app.MapGet("/Teste", async (VendasDBContext dbContext) =>
{
    // Test the database connection
    try
    {
        await dbContext.Database.OpenConnectionAsync();
        await dbContext.Database.CloseConnectionAsync();
        return Results.Ok("Database connection successful!");
    }
    catch (Exception ex)
    {
        return Results.Problem($"Database connection failed: {ex.Message}");
    }
});
#endregion


app.Run();