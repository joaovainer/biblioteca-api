import { createContext, useContext, useEffect, useState } from 'react'
import { apiFetch, getToken, setToken } from '../api/client'

const AuthContext = createContext(null)

export function AuthProvider({ children }) {
  const [user, setUser] = useState(null)
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    const token = getToken()
    if (!token) {
      setLoading(false)
      return
    }
    apiFetch('/api/auth/me')
      .then(setUser)
      .catch(() => setUser(null))
      .finally(() => setLoading(false))

    const onLogout = () => setUser(null)
    window.addEventListener('auth:logout', onLogout)
    return () => window.removeEventListener('auth:logout', onLogout)
  }, [])

  async function login(email, senha) {
    const data = await apiFetch('/api/auth/login', {
      method: 'POST',
      body: { email, senha },
      auth: false,
    })
    setToken(data.token)
    setUser({ nome: data.nome, email: data.email })
    return data
  }

  async function register(nome, email, senha) {
    const data = await apiFetch('/api/auth/register', {
      method: 'POST',
      body: { nome, email, senha },
      auth: false,
    })
    setToken(data.token)
    setUser({ nome: data.nome, email: data.email })
    return data
  }

  function logout() {
    setToken(null)
    setUser(null)
  }

  return (
    <AuthContext.Provider value={{ user, loading, login, register, logout }}>
      {children}
    </AuthContext.Provider>
  )
}

export function useAuth() {
  const ctx = useContext(AuthContext)
  if (!ctx) throw new Error('useAuth deve estar dentro de AuthProvider')
  return ctx
}
