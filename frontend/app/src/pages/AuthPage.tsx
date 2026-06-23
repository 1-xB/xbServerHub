import React from 'react'
import TwoFactorForm from '../components/auth/TwoFactorForm'
import LoginForm from '../components/auth/LoginForm'

interface AuthPageProps {
  setIsLoggedIn: (val: boolean) => void
}

function AuthPage({ setIsLoggedIn }: AuthPageProps) {
  const [show2FA, setShow2FA] = React.useState(false)



  if (show2FA) {
    return (
      <TwoFactorForm setIsLoggedIn={setIsLoggedIn}/>
    )
  }

  return (
    <LoginForm setIsLoggedIn={setIsLoggedIn} onRequires2FA={() => setShow2FA(true)} />
  )
}

export default AuthPage