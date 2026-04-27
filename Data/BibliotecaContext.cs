using Microsoft.EntityFrameworkCore;
using BibliotecaAPI.Models;

namespace BibliotecaAPI.Data;

public class BibliotecaContext : DbContext
{
    public BibliotecaContext(DbContextOptions<BibliotecaContext> options)
        : base(options)
    {
    }

    public DbSet<Livro> Livros { get; set; }
    public DbSet<Emprestimo> Emprestimos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuração do relacionamento
        modelBuilder.Entity<Emprestimo>()
            .HasOne(e => e.Livro)
            .WithMany(l => l.Emprestimos)
            .HasForeignKey(e => e.LivroId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Livro>()
            .HasKey(l => l.Id);

        modelBuilder.Entity<Emprestimo>()
            .HasKey(e => e.Id);
    }
}
