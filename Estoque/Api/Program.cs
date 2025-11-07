using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;
using Infrastructure;

/*
* ASP.NET Core Web API application setup micro serviço Estoque .
* Configures services, database context, Swagger for API documentation,
* and a test endpoint to verify database connectivity.
* Author: Paulo Daniel
* Date: Novembro de 2025
*/

var builder = WebApplication.CreateBuilder(args);

#region builder
    builder.Services.AddControllers();
    builder.Services.AddDbContext<EstoqueDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EstoqueConnection")));
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddInfrastructure();

    builder.Services.AddSwaggerGen();
    var app = builder.Build();
#endregion

#region swagger
    app.UseSwagger();
    app.UseSwaggerUI();
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
    });
#endregion

app.MapControllers();


app.Run();



