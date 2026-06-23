import React, { useState, type FormEvent } from 'react'
import { useNavigate } from 'react-router-dom';
import { loginWith2FA } from '../../services/authService';

interface TwoFactorFormProps {
    setIsLoggedIn: (val: boolean) => void
}

function TwoFactorForm({ setIsLoggedIn }: TwoFactorFormProps) {
    const navigate = useNavigate();
    const [twoFACode, setTwoFACode] = useState('');
    const [rememberMe, setRememberMe] = useState(false);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const handle2FASubmit = async (e: FormEvent) => {
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

export default TwoFactorForm