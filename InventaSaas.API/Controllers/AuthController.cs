using InventaSaas.Application.DTOs.Auth;
using InventaSaas.Application.Services.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventaSaas.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            var resultado = await _authService.Registrar(dto);
            if (resultado == "EMAIL_EXISTENTE")
                return BadRequest(new { mensaje = "El email ya está registrado" });

            return Ok(new { mensaje = "Usuario registrado con éxito" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var token = await _authService.Login(dto);
            if (token == null)
                return Unauthorized(new { mensaje = "Credenciales incorrectas" });

            return Ok(new { token });
        }

    }
}
