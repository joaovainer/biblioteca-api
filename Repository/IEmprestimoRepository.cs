using BibliotecaAPI.Models;

namespace BibliotecaAPI.Repository;

public interface IEmprestimoRepository
{
    Task<IEnumerable<object>> GetAllAsync();
    Task<object?> GetByIdAsync(int id);
    Task<Emprestimo> AddAsync(Emprestimo emprestimo);
    Task<Emprestimo> UpdateAsync(int id, Emprestimo emprestimo);
    Task<bool> DeleteAsync(int id);
}
