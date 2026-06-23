import React from 'react'
import { useNavigate } from 'react-router-dom'
import { loginUser, loginWith2FA } from '../services/authService'

interface AuthPageProps {
  setIsLoggedIn: (val: boolean) => void
}

function AuthPage({ setIsLoggedIn }: AuthPageProps) {
  const navigate = useNavigate()
  const [loading, setLoading] = React.useState(false)
  const [error, setError] = React.useState<string | null>(null)
  const [email, setEmail] = React.useState('')
  const [password, setPassword] = React.useState('')
  const [rememberMe, setRememberMe] = React.useState(false)
  const [show2FA, setShow2FA] = React.useState(false)
  const [twoFACode, setTwoFACode] = React.useState('')

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setLoading(true)
    setError(null)

    const result = await loginUser({email, password});
    if (result.requiresTwoFactor) {
      setShow2FA(true)
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

  const handle2FASubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setLoading(true)
    setError(null)

    if (twoFACode.trim() === '') {
      setError('Please enter the 2FA code.')
      setLoading(false)
      return
    }

    const result = await loginWith2FA({ code: twoFACode, rememberDevice: rememberMe });
    if (result.isSuccess) {
      setIsLoggedIn(true)
      navigate('/dashboard')
    }
    else {
      setError(result.message)
      setLoading(false)
    }
  }

  if (show2FA) {
    return (
      <div>
        <h1>xbServerHub</h1>
        <h2>Two-Factor Authentication</h2>
        <p>Please enter your 2FA code.</p>

        <form onSubmit={handle2FASubmit}>
          <div>
            <label htmlFor="twoFACode">2FA Code:</label>
            <input type="text" id="twoFACode" value={twoFACode} onChange={(e) => setTwoFACode(e.target.value)} required />
          </div>

          <div>
            <label htmlFor="rememberMe">Remember device</label>
            <input type="checkbox" id="rememberMe" checked={rememberMe} onChange={(e) => setRememberMe(e.target.checked)} />
          </div>
          {error && <p>{error}</p>}

          <button type="submit" disabled={loading}>
            {loading ? 'Verifying...' : 'Verify'}
          </button>
        </form>
      </div>
    )
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

export default AuthPage