
const express = require('express');
const router = express.Router();
const db = require('../db');


router.get('/', (req, res) => {
  const query = `
    SELECT e.id, e.livro_id, l.titulo AS nome_livro, e.nome_usuario, e.data_emprestimo, e.data_devolucao
    FROM emprestimos e
    JOIN livros l ON e.livro_id = l.id
  `;
  db.all(query, [], (err, rows) => {
    if (err) return res.send("Erro");
    res.json(rows);
  });
});


router.post('/', (req, res) => {
  const { livro_id, nome_usuario, data_emprestimo, data_devolucao } = req.body;
  db.run(
    "INSERT INTO emprestimos (livro_id, nome_usuario, data_emprestimo, data_devolucao) VALUES (?, ?, ?, ?)",
    [livro_id, nome_usuario, data_emprestimo, data_devolucao],
    function (err) {
      if (err) return res.send("Erro");
      res.json({ id: this.lastID });
    }
  );
});


router.put('/:id', (req, res) => {
  const { data_devolucao } = req.body;
  const id = req.params.id;
  db.run("UPDATE emprestimos SET data_devolucao = ? WHERE id = ?", [data_devolucao, id], function(err) {
    if (err) return res.send("Erro");
    if (this.changes === 0) return res.send("Não encontrado");
    res.json({ id });
  });
});



router.delete('/:id', (req, res) => {
  const id = req.params.id;
  db.run("DELETE FROM emprestimos WHERE id = ?", [id], function(err) {
    if (err) return res.send("Erro");
    if (this.changes === 0) return res.send("Não encontrado");
    res.send("Removido");
  });
});


module.exports = router;
