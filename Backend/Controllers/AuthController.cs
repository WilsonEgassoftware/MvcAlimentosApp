using Microsoft.AspNetCore.Mvc;
using SupermarketAPI.DTOs;
using SupermarketAPI.Services;

namespace SupermarketAPI.Controllers
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

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            var response = await _authService.LoginAsync(request);
            if (response == null)
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }

            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<ActionResult<LoginResponse>> Register([FromBody] RegisterRequest request)
        {
            var response = await _authService.RegisterAsync(request);
            if (response == null)
            {
                return BadRequest(new { message = "Username or email already exists" });
            }

            return Ok(response);
        }

        [HttpPost("verify-email")]
        public async Task<ActionResult> VerifyEmail([FromBody] VerifyEmailRequest request)
        {
            var result = await _authService.VerifyEmailAsync(request.Token);
            if (!result)
            {
                return BadRequest(new { message = "Token de verificación inválido o expirado" });
            }

            return Ok(new { message = "Email verificado exitosamente" });
        }

        [HttpGet("verify-email")]
        public async Task<ActionResult> VerifyEmailGet([FromQuery] string token)
        {
            var result = await _authService.VerifyEmailAsync(token);
            if (!result)
            {
                return BadRequest(new { message = "Token de verificación inválido o expirado" });
            }

            return Ok(new { message = "Email verificado exitosamente" });
        }
    }
}
