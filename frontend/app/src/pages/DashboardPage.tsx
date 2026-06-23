import { useNavigate } from 'react-router-dom'

interface DashboardPageProps {
  setIsLoggedIn: (val: boolean) => void
}

function DashboardPage({ setIsLoggedIn }: DashboardPageProps) {
    const navigate = useNavigate()
    const handleLogout = () => {
        setIsLoggedIn(false)
        navigate("/auth")
    }

  return (
    <div>
      <h1>xbServerHub</h1>
      <p>Welcome to your dashboard!</p>
      <button onClick={handleLogout}>Log Out</button>
    </div>
  )
}

export default DashboardPage