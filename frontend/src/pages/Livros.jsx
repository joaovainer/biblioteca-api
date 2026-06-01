import { useEffect, useState } from 'react'
import { apiFetch } from '../api/client'

export default function Livros() {
  const [livros, setLivros] = useState([])
  const [titulo, setTitulo] = useState('')
  const [autor, setAutor] = useState('')
  const [editandoId, setEditandoId] = useState(null)
  const [carregando, setCarregando] = useState(true)
  const [erro, setErro] = useState('')

  useEffect(() => {
    carregar()
  }, [])

  async function carregar() {
    setCarregando(true)
    setErro('')
    try {
      const data = await apiFetch('/api/livros')
      setLivros(data)
    } catch (err) {
      setErro(err.message)
    } finally {
      setCarregando(false)
    }
  }

  function resetForm() {
    setTitulo('')
    setAutor('')
    setEditandoId(null)
  }

  async function handleSubmit(e) {
    e.preventDefault()
    setErro('')
    try {
      if (editandoId) {
        await apiFetch(`/api/livros/${editandoId}`, {
          method: 'PUT',
          body: { titulo, autor },
        })
      } else {
        await apiFetch('/api/livros', {
          method: 'POST',
          body: { titulo, autor },
        })
      }
      resetForm()
      carregar()
    } catch (err) {
      setErro(err.message)
    }
  }

  function startEdit(livro) {
    setEditandoId(livro.id)
    setTitulo(livro.titulo)
    setAutor(livro.autor)
  }

  async function handleDelete(id) {
    if (!confirm('Remover este livro?')) return
    try {
      await apiFetch(`/api/livros/${id}`, { method: 'DELETE' })
      carregar()
    } catch (err) {
      setErro(err.message)
    }
  }

  return (
    <div className="page">
      <h1>Livros</h1>

      <form className="card" onSubmit={handleSubmit}>
        <h2>{editandoId ? 'Editar livro' : 'Novo livro'}</h2>
        <div className="form-row">
          <input
            type="text"
            placeholder="Título"
            value={titulo}
            onChange={(e) => setTitulo(e.target.value)}
            required
          />
          <input
            type="text"
            placeholder="Autor"
            value={autor}
            onChange={(e) => setAutor(e.target.value)}
            required
          />
          <button type="submit">{editandoId ? 'Salvar' : 'Adicionar'}</button>
          {editandoId && (
            <button type="button" onClick={resetForm} className="secondary">
              Cancelar
            </button>
          )}
        </div>
      </form>

      {erro && <div className="erro">{erro}</div>}

      <div className="card">
        {carregando ? (
          <p>Carregando...</p>
        ) : livros.length === 0 ? (
          <p>Nenhum livro cadastrado.</p>
        ) : (
          <table>
            <thead>
              <tr>
                <th>ID</th>
                <th>Título</th>
                <th>Autor</th>
                <th>Ações</th>
              </tr>
            </thead>
            <tbody>
              {livros.map((livro) => (
                <tr key={livro.id}>
                  <td>{livro.id}</td>
                  <td>{livro.titulo}</td>
                  <td>{livro.autor}</td>
                  <td className="acoes">
                    <button onClick={() => startEdit(livro)}>Editar</button>
                    <button onClick={() => handleDelete(livro.id)} className="danger">
                      Excluir
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </div>
    </div>
  )
}
