using Habitus.Api.DTOs;
using Habitus.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Habitus.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var result = await _authService.RegisterAsync(request);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            if (result.ErrorType == AuthErrorType.Conflict)
            {
                return Conflict(new { mensagem = result.Message });
            }

            return BadRequest(new { mensagem = result.Message });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var result = await _authService.LoginAsync(request);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            if (result.ErrorType == AuthErrorType.Unauthorized)
            {
                return Unauthorized(new { mensagem = result.Message });
            }

            return BadRequest(new { mensagem = result.Message });
        }
    }
}
