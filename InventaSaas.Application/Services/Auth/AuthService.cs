using InventaSaas.Application.DTOs.Auth;
using InventaSaas.Domain.Entities;
using InventaSaas.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InventaSaas.Application.Services.Auth
{
    public class AuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IConfiguration _config;

        public AuthService(IUsuarioRepository usuarioRepository, IConfiguration config)
        {
            _usuarioRepository = usuarioRepository;
            _config = config;
        }

        public async Task<string> Registrar(RegisterDTO dto)
        {
            var existe = await _usuarioRepository.GetByEmail(dto.Email);
            if (existe != null)
                return "EMAIL_EXISTENTE";

            var usuario = new Usuario
            {
                Nombres = dto.Nombres,
                Apellidos = dto.Apellidos,
                Email = dto.Email,
                Telefono = dto.Telefono,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                IdRol = 2,
                Activo = true
            };

            await _usuarioRepository.CrearUsuario(usuario);
            return "USUARIO_CREADO";
        }

        public async Task<string?> Login(LoginDTO dto)
        {
            var usuario = await _usuarioRepository.GetByEmail(dto.Email);
            if (usuario == null)
                return null;

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, usuario.PasswordHash))
                return null;

            return GenerarToken(usuario);
        }

        private string GenerarToken(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Name, $"{usuario.Nombres} {usuario.Apellidos}"),
                new Claim(ClaimTypes.Role, usuario.IdRol == 1 ? "Administrador" : "ClienteInvitado")
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(
                    double.Parse(_config["Jwt:ExpiresInHours"]!)),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}