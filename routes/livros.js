
const express = require('express');
const router = express.Router();
const db = require('../db');


router.get('/', (req, res) => {
  db.all("SELECT * FROM livros", [], (err, rows) => {
    if (err) return res.send("Erro");
    res.json(rows);
  });
});


router.post('/', (req, res) => {
  const { titulo, autor } = req.body;
  db.run(
    "INSERT INTO livros (titulo, autor) VALUES (?, ?)",
    [titulo, autor],
    function (err) {
      if (err) return res.send("Erro");
      res.json({ id: this.lastID });
    }
  );
});


router.put('/:id', (req, res) => {
  const { titulo, autor } = req.body;
  const id = req.params.id;
  db.run("UPDATE livros SET titulo = ?, autor = ? WHERE id = ?", [titulo, autor, id], function(err) {
    if (err) return res.send("Erro");
    if (this.changes === 0) return res.send("Não encontrado");
    res.json({ id });
  });
});


router.delete('/:id', (req, res) => {
  const id = req.params.id;
  db.run("DELETE FROM livros WHERE id = ?", [id], function(err) {
    if (err) return res.send("Erro");
    if (this.changes === 0) return res.send("Não encontrado");
    res.send("Removido");
  });
});

module.exports = router;
