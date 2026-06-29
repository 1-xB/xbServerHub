import { useNavigate } from 'react-router-dom'
import { logoutUser } from '../services/authService'
import { useMetricsHub } from '../hooks/useMetricsHub'

interface DashboardPageProps {
  setIsLoggedIn: (val: boolean) => void
}

function DashboardPage({ setIsLoggedIn }: DashboardPageProps) {
  const metrics = useMetricsHub()
    const navigate = useNavigate()
    const handleLogout = () => {
        logoutUser()
        setIsLoggedIn(false)
        navigate("/auth")
    }
    
  
  
  return (
    <div>
      <p>CPU Usage: {metrics?.cpu?.usagePercent ?? "—"}%</p>
      <h1>xbServerHub</h1>
      <p>Welcome to your dashboard!</p>
      <button onClick={handleLogout}>Log Out</button>
    </div>
  )
}

export default DashboardPage