using BibliotecaAPI.Models;

namespace BibliotecaAPI.Repository;

public interface ILivroRepository
{
    Task<IEnumerable<Livro>> GetAllAsync();
    Task<Livro?> GetByIdAsync(int id);
    Task<Livro> AddAsync(Livro livro);
    Task<Livro> UpdateAsync(int id, Livro livro);
    Task<bool> DeleteAsync(int id);
}
