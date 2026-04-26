
const sqlite3 = require('sqlite3').verbose();
const db = new sqlite3.Database('./biblioteca.db');

db.run(`CREATE TABLE IF NOT EXISTS livros (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  titulo TEXT,
  autor TEXT
)`);

db.run(`CREATE TABLE IF NOT EXISTS emprestimos (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  livro_id INTEGER,
  nome_usuario TEXT,
  data_emprestimo TEXT,
  data_devolucao TEXT,
  FOREIGN KEY(livro_id) REFERENCES livros(id)
)`);

module.exports = db;
