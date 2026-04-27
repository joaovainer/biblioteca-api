namespace BibliotecaAPI.Models;

public class Emprestimo
{
    public int Id { get; set; }
    public int LivroId { get; set; }
    public string NomeUsuario { get; set; } = string.Empty;
    public DateTime DataEmprestimo { get; set; }
    public DateTime? DataDevolucao { get; set; }
    public StatusEmprestimo Status { get; set; } = StatusEmprestimo.Ativo;
    // Relacionamento
    public Livro? Livro { get; set; }
}
