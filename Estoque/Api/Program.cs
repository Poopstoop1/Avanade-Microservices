using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); 
builder.Services.AddDbContext<EstoqueDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EstoqueConnection")));

var app = builder.Build();

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
});

app.Run();



