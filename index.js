
const express = require('express');
const app = express();
const db = require('./db');

app.use(express.json());

app.use('/emprestimos', require('./routes/emprestimos'));
app.use('/livros', require('./routes/livros'));

app.get('/', (req, res) => {
  res.send('API da Biblioteca!');
});

app.delete('/limpar', (req, res) => {
  db.run('DELETE FROM emprestimos', [], (err) => {
    if (err) return res.send('Erro');

    db.run('DELETE FROM livros', [], (err2) => {
      if (err2) return res.send('Erro');

      db.run("DELETE FROM sqlite_sequence WHERE name IN ('emprestimos', 'livros')", [], (err3) => {
        if (err3) return res.send('Erro');
        res.send('Todos os livros e empréstimos foram removidos');
      });
    });
  });
});

app.listen(4000);
