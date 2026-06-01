using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BibliotecaAPI.Models;
using BibliotecaAPI.Repository;

namespace BibliotecaAPI.Controllers;

[ApiController]
[Authorize]
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
            return BadRequest(new { mensagem = "Dados obrigatórios ausentes ou inválidos." });

        var emprestimo = new Emprestimo
        {
            LivroId = dto.LivroId.Value,
            NomeUsuario = dto.NomeUsuario!,
            DataEmprestimo = dto.DataEmprestimo.Value,
            DataDevolucao = dto.DataDevolucao,
            Status = dto.DataDevolucao != null ? StatusEmprestimo.Devolvido : StatusEmprestimo.Ativo
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
            await _repository.UpdateAsync(id, emprestimo);
            var resultado = await _repository.GetByIdAsync(id);
            return Ok(resultado);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPatch("{id}/devolver")]
    public async Task<IActionResult> Devolver(int id)
    {
        var atualizado = await _repository.DevolverAsync(id);
        if (atualizado == null)
            return NotFound();

        var resultado = await _repository.GetByIdAsync(id);
        return Ok(resultado);
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
