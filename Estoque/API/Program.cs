using API;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

/*
* ASP.NET Core Web API application setup micro serviço Estoque .
* Configures services, database context, Swagger for API documentation,
* and a test endpoint to verify database connectivity.
* Author: Paulo Daniel
* Date: Novembro de 2025
*/

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();


startup.Configure(app, app.Environment);


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



