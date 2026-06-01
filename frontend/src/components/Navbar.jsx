import { NavLink, useNavigate } from 'react-router-dom'
import { useAuth } from '../context/AuthContext'

export default function Navbar() {
  const { user, logout } = useAuth()
  const navigate = useNavigate()

  if (!user) return null

  function handleLogout() {
    logout()
    navigate('/login')
  }

  return (
    <nav className="navbar">
      <div className="navbar-brand">Biblioteca</div>
      <div className="navbar-links">
        <NavLink to="/livros" className={({ isActive }) => isActive ? 'active' : ''}>Livros</NavLink>
        <NavLink to="/emprestimos" className={({ isActive }) => isActive ? 'active' : ''}>Empréstimos</NavLink>
      </div>
      <div className="navbar-user">
        <span>{user.nome}</span>
        <button onClick={handleLogout}>Sair</button>
      </div>
    </nav>
  )
}
