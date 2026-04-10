using API.Extensions;
using Application;
using Infrastructure;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

/*
* ASP.NET Core Web API application setup micro serviço Estoque .
* Configures services, database context, Swagger for API documentation,
* and a test endpoint to verify database connectivity.
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
    });
builder.Services.AddMessaging();
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
app.MapGet("/Teste", async (EstoqueDBContext dbContext) =>
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
        }).WithTags("DatabaseTeste").WithDescription("Endpoint para testar o Banco de Dados");
#endregion



app.Run();



