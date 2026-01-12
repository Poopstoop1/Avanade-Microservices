using Application;
using Infrastructure;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
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
builder.Services.AddSwaggerDocumentation();
builder.Services.AddAuthenticationJwt(builder.Configuration);
builder.Services.AddMessaging();
builder.Services.AddRedisCache();
#endregion



#region AppSettings
var app = builder.Build();
app.UseHttpsRedirection();
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