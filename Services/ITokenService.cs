using BibliotecaAPI.Models;

namespace BibliotecaAPI.Services;

public interface ITokenService
{
    string GenerateToken(Usuario usuario);
}
