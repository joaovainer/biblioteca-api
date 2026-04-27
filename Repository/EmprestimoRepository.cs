using Microsoft.EntityFrameworkCore;
using BibliotecaAPI.Models;
using BibliotecaAPI.Data;

namespace BibliotecaAPI.Repository;

public class EmprestimoRepository : IEmprestimoRepository
{
    private readonly BibliotecaContext _context;

    public EmprestimoRepository(BibliotecaContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<object>> GetAllAsync()
    {
        return await _context.Emprestimos
            .Include(e => e.Livro)
            .Select(e => new
            {
                e.Id,
                e.LivroId,
                NomeLivro = e.Livro!.Titulo,
                e.NomeUsuario,
                e.DataEmprestimo,
                e.DataDevolucao
            })
            .ToListAsync();
    }

    public async Task<Emprestimo?> GetByIdAsync(int id)
    {
        return await _context.Emprestimos
            .Include(e => e.Livro)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<Emprestimo> AddAsync(Emprestimo emprestimo)
    {
        _context.Emprestimos.Add(emprestimo);
        await _context.SaveChangesAsync();
        return emprestimo;
    }

    public async Task<Emprestimo> UpdateAsync(int id, Emprestimo emprestimo)
    {
        var existente = await _context.Emprestimos.FindAsync(id);
        if (existente == null)
            throw new KeyNotFoundException($"Empréstimo com ID {id} não encontrado");

        existente.DataDevolucao = emprestimo.DataDevolucao;
        if (emprestimo.DataDevolucao != null)
            existente.Status = StatusEmprestimo.Devolvido;

        _context.Emprestimos.Update(existente);
        await _context.SaveChangesAsync();
        return existente;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var emprestimo = await _context.Emprestimos.FindAsync(id);
        if (emprestimo == null)
            return false;

        _context.Emprestimos.Remove(emprestimo);
        await _context.SaveChangesAsync();
        return true;
    }
}
