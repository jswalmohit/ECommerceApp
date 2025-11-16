using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        [HttpGet("/env")]
        public IActionResult EnvTest()
        {
            //return Ok("Env test successful");
            var test = Environment.GetEnvironmentVariable("testConnString") ?? "NULL";
            //var cs = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection") ?? "NULL";

            return Ok(new
            {
                test = test,
                //connstring = cs
            });
        }
    }
}
