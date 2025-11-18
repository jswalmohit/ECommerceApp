using ECommerceApp.Context;
using ECommerceApp.Extension;
using ECommerceApp.Middleware;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog for enhanced logging
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
    .MinimumLevel.Override("System", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .Enrich.WithThreadId()
    .WriteTo.Console()
    .WriteTo.File(
        new JsonFormatter(),
        path: "logs/ecommerce-.log",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 30,
        shared: true)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddServices();
builder.Services.AddJwtAuthentication(builder);
builder.Services.AddCorsConfig();
builder.Services.AddDbContext<EComDbContext>(options =>
{
    var conn = builder.Configuration.GetConnectionString("DefaultConnection")
               ?? Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");

    options.UseSqlServer(conn);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

// Add global exception handler middleware (should be early in the pipeline)
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

try
{
    Log.Information("Starting ECommerceApp");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
