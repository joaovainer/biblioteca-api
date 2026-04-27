using Microsoft.AspNetCore.Mvc;
using BibliotecaAPI.Models;
using BibliotecaAPI.Repository;

namespace BibliotecaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmprestimosController : ControllerBase
{
    private readonly IEmprestimoRepository _repository;

    public EmprestimosController(IEmprestimoRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var emprestimos = await _repository.GetAllAsync();
        return Ok(emprestimos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var emprestimo = await _repository.GetByIdAsync(id);
        if (emprestimo == null)
            return NotFound();
        return Ok(emprestimo);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] EmprestimoDto dto)
    {
        if (dto.LivroId == null || dto.LivroId <= 0 || string.IsNullOrWhiteSpace(dto.NomeUsuario) || dto.DataEmprestimo == null)
        return BadRequest("Dados obrigatórios ausentes ou inválidos.");

        var emprestimo = new Emprestimo
        {
            LivroId = dto.LivroId.Value,
            NomeUsuario = dto.NomeUsuario!,
            DataEmprestimo = dto.DataEmprestimo.Value,
            DataDevolucao = dto.DataDevolucao,
            Status = StatusEmprestimo.Ativo
        };

        await _repository.AddAsync(emprestimo);

        var resultado = await _repository.GetByIdAsync(emprestimo.Id);

        return CreatedAtAction(nameof(GetById), new { id = emprestimo.Id }, resultado);
}

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] Emprestimo emprestimo)
    {
        try
        {
            var emprestimoAtualizado = await _repository.UpdateAsync(id, emprestimo);
            return Ok(emprestimoAtualizado);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var resultado = await _repository.DeleteAsync(id);
        if (!resultado)
            return NotFound();
        return Ok();
    }
}
