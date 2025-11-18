using Api;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
       .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();


startup.Configure(app,app.Environment);


app.Run();


