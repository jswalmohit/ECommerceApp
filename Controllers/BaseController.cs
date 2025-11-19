using ECommerceApp.EComm.Commons.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerceApp.Controllers
{
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        internal int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedAccessException("Invalid user ID in token");
            }
            return userId;
        }

        protected string? GetUserEmail()
        {
            return User.FindFirst(ClaimTypes.Email)?.Value;
        }

        protected string? GetUserLoginId()
        {
            return User.FindFirst(ClaimTypes.Name)?.Value;
        }

        internal IActionResult HandleResult<T>(ServiceResult<T> result)
        {
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }

            if (result.ValidationErrors != null && result.ValidationErrors.Any())
            {
                return BadRequest(new { errors = result.ValidationErrors, message = result.ErrorMessage });
            }

            return result.ErrorCode switch
            {
                404 => NotFound(new { message = result.ErrorMessage }),
                401 => Unauthorized(new { message = result.ErrorMessage }),
                403 => Forbid(),
                _ => BadRequest(new { message = result.ErrorMessage })
            };
        }

        internal IActionResult HandleResult(ServiceResult result)
        {
            if (result.IsSuccess)
            {
                return Ok(new { message = "Operation completed successfully" });
            }

            if (result.ValidationErrors != null && result.ValidationErrors.Any())
            {
                return BadRequest(new { errors = result.ValidationErrors, message = result.ErrorMessage });
            }

            return result.ErrorCode switch
            {
                404 => NotFound(new { message = result.ErrorMessage }),
                401 => Unauthorized(new { message = result.ErrorMessage }),
                403 => Forbid(),
                _ => BadRequest(new { message = result.ErrorMessage })
            };
        }
    }
}

