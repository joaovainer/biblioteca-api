import { BrowserRouter, Navigate, Route, Routes } from 'react-router-dom'
import { AuthProvider } from './context/AuthContext'
import Navbar from './components/Navbar'
import ProtectedRoute from './components/ProtectedRoute'
import Login from './pages/Login'
import Livros from './pages/Livros'
import Emprestimos from './pages/Emprestimos'
import './App.css'

export default function App() {
  return (
    <BrowserRouter>
      <AuthProvider>
        <Navbar />
        <main className="app-main">
          <Routes>
            <Route path="/login" element={<Login />} />
            <Route
              path="/livros"
              element={
                <ProtectedRoute>
                  <Livros />
                </ProtectedRoute>
              }
            />
            <Route
              path="/emprestimos"
              element={
                <ProtectedRoute>
                  <Emprestimos />
                </ProtectedRoute>
              }
            />
            <Route path="*" element={<Navigate to="/livros" replace />} />
          </Routes>
        </main>
      </AuthProvider>
    </BrowserRouter>
  )
}
