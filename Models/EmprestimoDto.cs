namespace BibliotecaAPI.Models;

public class EmprestimoDto
{
    public int? LivroId { get; set; }
    public string? NomeUsuario { get; set; }
    public DateTime? DataEmprestimo { get; set; }
    public DateTime? DataDevolucao { get; set; }
}
