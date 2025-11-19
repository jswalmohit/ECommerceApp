using ECommerceApp.EComm.Commons.Modals;
using ECommerceApp.EComm.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController(IRegisterService registerService) : ControllerBase
    {
        private readonly IRegisterService _registerService = registerService;

        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] UserRequest user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdUser = await _registerService.CreateUserAsync(user);
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserRequest user)
        {
            var updatedUser = await _registerService.UpdateUserAsync(id, user);
            if (updatedUser == null)
                return NotFound($"User with ID {id} not found.");

            return Ok(updatedUser);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _registerService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }
    }
}

