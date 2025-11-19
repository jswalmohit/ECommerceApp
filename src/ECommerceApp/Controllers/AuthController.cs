using ECommerceApp.EComm.Commons.Modals;
using ECommerceApp.EComm.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("token")]
        public async Task<IActionResult> GenerateJwtToken([FromBody] LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var authResponse = await _authService.GenerateJwtTokenAsync(loginRequest);

            if (authResponse == null)
                return Unauthorized("Invalid login credentials");

            return Ok(authResponse);
        }
    }
}

