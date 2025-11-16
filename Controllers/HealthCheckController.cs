using ECommerceApp.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Controllers { 

[ApiController]
[Route("health")]
public class HealthCheckController : ControllerBase
{
    private readonly EComDbContext _context;

    public HealthCheckController(EComDbContext context)
    {
        _context = context;
    }

    [HttpGet("db")]
    public async Task<IActionResult> CheckDatabase()
    {
        try
        {
            // Simple query to test DB
            await _context.Database.ExecuteSqlRawAsync("SELECT 1");

            return Ok(new
            {
                status = "OK",
                message = "Database connection successful"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                status = "ERROR",
                message = ex.Message
            });
        }
    }
}
}