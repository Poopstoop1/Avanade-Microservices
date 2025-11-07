using Vendas.Infrastructure.DB;
using Microsoft.EntityFrameworkCore;
using Vendas.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

#region builder
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<VendasDBContext>(options =>
   options.UseSqlServer(builder.Configuration.GetConnectionString("VendasConnection")));
var app = builder.Build();
#endregion


#region swagger
    app.UseSwagger();
    app.UseSwaggerUI();
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