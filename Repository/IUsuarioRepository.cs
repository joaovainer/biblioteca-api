using BibliotecaAPI.Models;

namespace BibliotecaAPI.Repository;

public interface IUsuarioRepository
{
    Task<Usuario?> GetByEmailAsync(string email);
    Task<Usuario?> GetByIdAsync(int id);
    Task<Usuario> CreateAsync(Usuario usuario);
    Task<bool> ExistsByEmailAsync(string email);
}
