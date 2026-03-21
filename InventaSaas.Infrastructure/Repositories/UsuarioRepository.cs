using InventaSaas.Domain.Entities;
using InventaSaas.Domain.Interfaces;
using InventaSaas.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InventaSaas.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> GetByEmail(string email)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(x => x.Email == email && x.Activo == true);
        }

        public async Task CrearUsuario(Usuario usuario)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC REGISTROLOGEO.registrar_usuario @nombres={0}, @apellidos={1}, @email={2}, @telefono={3}, @password_hash={4}, @id_Rol={5}",
                usuario.Nombres,
                usuario.Apellidos,
                usuario.Email,
                usuario.Telefono,
                usuario.PasswordHash,
                usuario.IdRol
            );
        }
    }
}