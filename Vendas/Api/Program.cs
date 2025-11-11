using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Vendas;
using Vendas.Infrastructure.DB;

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();


startup.Configure(app, app.Environment);


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