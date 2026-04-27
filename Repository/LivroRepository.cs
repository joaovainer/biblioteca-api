using Microsoft.EntityFrameworkCore;
using BibliotecaAPI.Models;
using BibliotecaAPI.Data;

namespace BibliotecaAPI.Repository;

public class LivroRepository : ILivroRepository
{
    private readonly BibliotecaContext _context;

    public LivroRepository(BibliotecaContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Livro>> GetAllAsync()
    {
        return await _context.Livros.ToListAsync();
    }

    public async Task<Livro?> GetByIdAsync(int id)
    {
        return await _context.Livros.FindAsync(id);
    }

    public async Task<Livro> AddAsync(Livro livro)
    {
        _context.Livros.Add(livro);
        await _context.SaveChangesAsync();
        return livro;
    }

    public async Task<Livro> UpdateAsync(int id, Livro livro)
    {
        var existente = await _context.Livros.FindAsync(id);
        if (existente == null)
            throw new KeyNotFoundException($"Livro com ID {id} não encontrado");

        existente.Titulo = livro.Titulo;
        existente.Autor = livro.Autor;

        _context.Livros.Update(existente);
        await _context.SaveChangesAsync();
        return existente;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var livro = await _context.Livros.FindAsync(id);
        if (livro == null)
            return false;

        _context.Livros.Remove(livro);
        await _context.SaveChangesAsync();
        return true;
    }
}
