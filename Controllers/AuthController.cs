using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BibliotecaAPI.Models;
using BibliotecaAPI.Models.Auth;
using BibliotecaAPI.Repository;
using BibliotecaAPI.Services;

namespace BibliotecaAPI.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IUsuarioRepository _usuarios;
    private readonly ITokenService _tokens;

    public AuthController(IUsuarioRepository usuarios, ITokenService tokens)
    {
        _usuarios = usuarios;
        _tokens = tokens;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Nome) || string.IsNullOrWhiteSpace(req.Email) || string.IsNullOrWhiteSpace(req.Senha))
            return BadRequest(new { mensagem = "Nome, email e senha são obrigatórios." });

        if (req.Senha.Length < 6)
            return BadRequest(new { mensagem = "A senha deve ter pelo menos 6 caracteres." });

        if (await _usuarios.ExistsByEmailAsync(req.Email))
            return Conflict(new { mensagem = "Email já cadastrado." });

        var usuario = new Usuario
        {
            Nome = req.Nome.Trim(),
            Email = req.Email.Trim().ToLowerInvariant(),
            SenhaHash = BCrypt.Net.BCrypt.HashPassword(req.Senha)
        };

        await _usuarios.CreateAsync(usuario);

        return Ok(new AuthResponse
        {
            Token = _tokens.GenerateToken(usuario),
            Nome = usuario.Nome,
            Email = usuario.Email
        });
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Email) || string.IsNullOrWhiteSpace(req.Senha))
            return BadRequest(new { mensagem = "Email e senha são obrigatórios." });

        var usuario = await _usuarios.GetByEmailAsync(req.Email.Trim().ToLowerInvariant());
        if (usuario == null || !BCrypt.Net.BCrypt.Verify(req.Senha, usuario.SenhaHash))
            return Unauthorized(new { mensagem = "Credenciais inválidas." });

        return Ok(new AuthResponse
        {
            Token = _tokens.GenerateToken(usuario),
            Nome = usuario.Nome,
            Email = usuario.Email
        });
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> Me()
    {
        var idClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub) ?? User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(idClaim, out var id))
            return Unauthorized();

        var usuario = await _usuarios.GetByIdAsync(id);
        if (usuario == null)
            return NotFound();

        return Ok(new { usuario.Id, usuario.Nome, usuario.Email });
    }
}
