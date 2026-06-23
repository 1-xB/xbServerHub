import React from 'react'
import { useNavigate } from 'react-router-dom'
import { loginUser } from '../../services/authService'

interface LoginFormProps {
    setIsLoggedIn: (val: boolean) => void
    onRequires2FA: () => void
}

function LoginForm({ setIsLoggedIn, onRequires2FA }: LoginFormProps) {
    const navigate = useNavigate()
    const [loading, setLoading] = React.useState(false)
    const [error, setError] = React.useState<string | null>(null)
    const [email, setEmail] = React.useState('')
    const [password, setPassword] = React.useState('')
    
    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault()
        setLoading(true)
        setError(null)
    
        const result = await loginUser({email, password});
        if (result.requiresTwoFactor) {
          onRequires2FA()
          setLoading(false)
        }
        else if (result.isSuccess) {
          setIsLoggedIn(true)
          navigate('/dashboard')
        }
        else {
          setError(result.message)
          setLoading(false)
        }
      }
  return (
    <div>
      <div>
        <h1>xbServerHub</h1>
        <p>Welcome to xbServerHub</p>

        <form onSubmit={handleSubmit}>
          <div>
            <label htmlFor="email">Email:</label>
            <input type="email" id="email" value={email} onChange={(e) => setEmail(e.target.value)} required placeholder="name@example.com"/>
          </div>

          <div>
            <label htmlFor="password">Password:</label>
            <input type="password" id="password" value={password} onChange={(e) => setPassword(e.target.value)} required placeholder="********"/>
          </div>

          {error && <p>{error}</p>}

          <button type="submit" disabled={loading}>
            {loading ? 'Logging in...' : 'Login'}
          </button>
        </form>
      </div>
    </div>
  )
}

export default LoginForm