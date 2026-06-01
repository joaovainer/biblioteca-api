# Biblioteca — Web API + React

Sistema de biblioteca com cadastro de livros, registro de empréstimos e autenticação JWT. Backend em **C# / ASP.NET Core 9** (EF Core + SQLite, Repository Pattern). Frontend em **React + Vite**.

## Integrantes da equipe

- Gabriel Leineker Wolff — 29563089
- João Emanuel Vainer de Paula — 39023532
- Lucas Gonçalves de Lima — 38107899
- Eduardo Luiz Lima Correia — 38746778

## Tecnologias

**Backend:** .NET 9 · EF Core 9 · SQLite · JWT Bearer · BCrypt · DotNetEnv
**Frontend:** React 19 · Vite · React Router · Fetch API · CSS puro

## Como rodar

Pré-requisitos: **.NET 9** e **Node 20+**.

**Backend** (porta 5000):
```bash
cp .env.example .env
dotnet run --urls=http://localhost:5000
```

**Frontend** (porta 5173, em outro terminal):
```bash
cd frontend
npm install
npm run dev
```

Abra `http://localhost:5173`, cadastre um usuário e use o sistema. O banco SQLite e a tabela `Usuarios` são criados na primeira execução.

## Endpoints

Tudo abaixo de `/api/livros` e `/api/emprestimos` exige `Authorization: Bearer <token>`. Exemplos prontos em [`requests.http`](./requests.http).

| Método | Rota | Auth |
|---|---|---|
| POST | `/api/auth/register` | ❌ |
| POST | `/api/auth/login` | ❌ |
| GET | `/api/auth/me` | ✅ |
| GET POST | `/api/livros` | ✅ |
| GET PUT DELETE | `/api/livros/{id}` | ✅ |
| GET POST | `/api/emprestimos` | ✅ |
| GET PUT DELETE | `/api/emprestimos/{id}` | ✅ |
| **PATCH** | `/api/emprestimos/{id}/devolver` | ✅ |

## Autenticação JWT

`POST /api/auth/login` valida a senha com **BCrypt** e devolve um JWT assinado em **HMAC-SHA256** com a chave definida em `.env` (`Jwt__Key`). O token carrega `sub` (id), `email`, `name` e `exp` (8h).

O frontend guarda o token em `localStorage` e o `apiFetch` (`frontend/src/api/client.js`) injeta `Authorization: Bearer <token>` em toda requisição. O backend valida assinatura, issuer, audience e expiração via middleware `AddJwtBearer`; controllers protegidos têm `[Authorize]`.

**Testar via Postman:** faça `POST /api/auth/login`, copie o `token`, e use `Authorization: Bearer <token>` nas demais requests. Confirme com `GET /api/auth/me`.
