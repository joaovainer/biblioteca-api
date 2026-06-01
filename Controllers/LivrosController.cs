using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BibliotecaAPI.Models;
using BibliotecaAPI.Repository;

namespace BibliotecaAPI.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class LivrosController : ControllerBase
{
    private readonly ILivroRepository _repository;

    public LivrosController(ILivroRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var livros = await _repository.GetAllAsync();
        return Ok(livros);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var livro = await _repository.GetByIdAsync(id);
        if (livro == null)
            return NotFound();
        return Ok(livro);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Livro livro)
    {
        if (string.IsNullOrWhiteSpace(livro.Titulo) || string.IsNullOrWhiteSpace(livro.Autor))
            return BadRequest();

        var novoLivro = await _repository.AddAsync(livro);
        return CreatedAtAction(nameof(GetById), new { id = novoLivro.Id }, novoLivro);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] Livro livro)
    {
        if (string.IsNullOrWhiteSpace(livro.Titulo) || string.IsNullOrWhiteSpace(livro.Autor))
            return BadRequest();

        try
        {
            var livroAtualizado = await _repository.UpdateAsync(id, livro);
            return Ok(livroAtualizado);
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
