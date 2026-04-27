using Microsoft.EntityFrameworkCore;
using BibliotecaAPI.Data;
using BibliotecaAPI.Repository;

var builder = WebApplication.CreateBuilder(args);

// Configurar DbContext
builder.Services.AddDbContext<BibliotecaContext>(options =>
    options.UseSqlite("Data Source=biblioteca.db"));

// Registrar Repositories
builder.Services.AddScoped<ILivroRepository, LivroRepository>();
builder.Services.AddScoped<IEmprestimoRepository, EmprestimoRepository>();

// Adicionar Controllers
builder.Services.AddControllers();

var app = builder.Build();

// Criar banco de dados automaticamente
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BibliotecaContext>();
    db.Database.EnsureCreated();
}

app.UseAuthorization();
app.MapControllers();

app.Run();
