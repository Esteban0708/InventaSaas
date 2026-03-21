using InventaSaas.Domain.Entities;

namespace InventaSaas.Domain.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario?> GetByEmail(string email);

    Task CrearUsuario(Usuario usuario);
}