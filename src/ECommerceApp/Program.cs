using ECommerceApp.EComm.Data.Context;
using ECommerceApp.EComm.Logging;
using ECommerceApp.Extension;
using ECommerceApp.Middleware;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureSerilog();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddServices();
builder.Services.AddJwtAuthentication(builder);
builder.Services.AddCorsConfig(builder.Configuration);
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

// Add CORS middleware (must be before UseAuthentication and UseAuthorization)
app.UseCors();

// Add global exception handler middleware (should be early in the pipeline)
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.UseMiddleware<HeaderValidationMiddleware>();

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
