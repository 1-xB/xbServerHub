import { useNavigate } from 'react-router-dom'

interface AuthPageProps {
  setIsLoggedIn: (val: boolean) => void
}

function AuthPage({ setIsLoggedIn }: AuthPageProps) {
  const navigate = useNavigate()

  const handleLogin = () => {
    setIsLoggedIn(true)
    navigate("/dashboard")
  }

  return (
    <div>
      <h1>xbServerHub</h1>
      <p>Welcome to xbServerHub! Please log in to continue.</p>
      <button onClick={handleLogin}>Log In</button>
    </div>
  )
}

export default AuthPage