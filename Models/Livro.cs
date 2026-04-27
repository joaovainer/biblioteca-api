namespace BibliotecaAPI.Models;

public class Livro
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Autor { get; set; } = string.Empty;

    // Relacionamento
    public ICollection<Emprestimo> Emprestimos { get; set; } = new List<Emprestimo>();
}
