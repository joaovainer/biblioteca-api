# Biblioteca API - C# ASP.NET

Sistema de gerenciamento de biblioteca com registro de livros e empréstimos.

## Tecnologias

- ASP.NET Core 8.0
- SQLite
- Entity Framework Core
- Repository Pattern

## Execução

```bash
dotnet restore
dotnet run
```

Servidor rodará em `http://localhost:5000`

## Endpoints

### Livros

- `GET /api/livros` - Listar todos
- `GET /api/livros/{id}` - Obter por ID
- `POST /api/livros` - Criar novo
- `PUT /api/livros/{id}` - Atualizar
- `DELETE /api/livros/{id}` - Remover

### Empréstimos

- `GET /api/emprestimos` - Listar todos
- `GET /api/emprestimos/{id}` - Obter por ID
- `POST /api/emprestimos` - Criar novo
- `PUT /api/emprestimos/{id}` - Atualizar
- `DELETE /api/emprestimos/{id}` - Remover

## Importar no Postman

Cole as requisições do arquivo `requests.http` no Postman para testar todos os endpoints.
