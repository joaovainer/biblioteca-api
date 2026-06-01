import { useEffect, useState } from 'react'
import { apiFetch } from '../api/client'

function hojeIso() {
  const d = new Date()
  d.setSeconds(0, 0)
  const off = d.getTimezoneOffset()
  const local = new Date(d.getTime() - off * 60000)
  return local.toISOString().slice(0, 16)
}

export default function Emprestimos() {
  const [emprestimos, setEmprestimos] = useState([])
  const [livros, setLivros] = useState([])
  const [livroId, setLivroId] = useState('')
  const [nomeUsuario, setNomeUsuario] = useState('')
  const [dataEmprestimo, setDataEmprestimo] = useState(hojeIso())
  const [carregando, setCarregando] = useState(true)
  const [erro, setErro] = useState('')

  useEffect(() => {
    carregar()
  }, [])

  async function carregar() {
    setCarregando(true)
    setErro('')
    try {
      const [emps, livs] = await Promise.all([
        apiFetch('/api/emprestimos'),
        apiFetch('/api/livros'),
      ])
      setEmprestimos(emps)
      setLivros(livs)
    } catch (err) {
      setErro(err.message)
    } finally {
      setCarregando(false)
    }
  }

  async function handleSubmit(e) {
    e.preventDefault()
    setErro('')
    if (!livroId) {
      setErro('Selecione um livro.')
      return
    }
    try {
      await apiFetch('/api/emprestimos', {
        method: 'POST',
        body: {
          livroId: Number(livroId),
          nomeUsuario,
          dataEmprestimo,
          dataDevolucao: null,
        },
      })
      setNomeUsuario('')
      setLivroId('')
      setDataEmprestimo(hojeIso())
      carregar()
    } catch (err) {
      setErro(err.message)
    }
  }

  async function handleDevolver(id) {
    try {
      await apiFetch(`/api/emprestimos/${id}/devolver`, { method: 'PATCH' })
      carregar()
    } catch (err) {
      setErro(err.message)
    }
  }

  async function handleDelete(id) {
    if (!confirm('Remover este empréstimo?')) return
    try {
      await apiFetch(`/api/emprestimos/${id}`, { method: 'DELETE' })
      carregar()
    } catch (err) {
      setErro(err.message)
    }
  }

  function formatDate(value) {
    if (!value) return '—'
    return new Date(value).toLocaleString('pt-BR')
  }

  return (
    <div className="page">
      <h1>Empréstimos</h1>

      <form className="card" onSubmit={handleSubmit}>
        <h2>Novo empréstimo</h2>
        <div className="form-row">
          <select value={livroId} onChange={(e) => setLivroId(e.target.value)} required>
            <option value="">Selecione um livro</option>
            {livros.map((l) => (
              <option key={l.id} value={l.id}>
                {l.titulo} — {l.autor}
              </option>
            ))}
          </select>
          <input
            type="text"
            placeholder="Nome do usuário"
            value={nomeUsuario}
            onChange={(e) => setNomeUsuario(e.target.value)}
            required
          />
          <input
            type="datetime-local"
            value={dataEmprestimo}
            onChange={(e) => setDataEmprestimo(e.target.value)}
            required
          />
          <button type="submit">Registrar</button>
        </div>
      </form>

      {erro && <div className="erro">{erro}</div>}

      <div className="card">
        {carregando ? (
          <p>Carregando...</p>
        ) : emprestimos.length === 0 ? (
          <p>Nenhum empréstimo cadastrado.</p>
        ) : (
          <table>
            <thead>
              <tr>
                <th>ID</th>
                <th>Livro</th>
                <th>Usuário</th>
                <th>Empréstimo</th>
                <th>Devolução</th>
                <th>Status</th>
                <th>Ações</th>
              </tr>
            </thead>
            <tbody>
              {emprestimos.map((e) => (
                <tr key={e.id}>
                  <td>{e.id}</td>
                  <td>{e.nomeLivro ?? `#${e.livroId}`}</td>
                  <td>{e.nomeUsuario}</td>
                  <td>{formatDate(e.dataEmprestimo)}</td>
                  <td>{formatDate(e.dataDevolucao)}</td>
                  <td>
                    <span className={`badge ${e.status === 'Devolvido' ? 'ok' : 'pending'}`}>
                      {e.status}
                    </span>
                  </td>
                  <td className="acoes">
                    {e.status !== 'Devolvido' && (
                      <button onClick={() => handleDevolver(e.id)}>Devolver</button>
                    )}
                    <button onClick={() => handleDelete(e.id)} className="danger">
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
