import LoginPage from './pages/LoginPage/LoginPage'
import ChatPage from './pages/ChatPage/ChatPage'
import { UseAuth } from './contexts/AuthContext'

function App() {
  const { loggedIn } = UseAuth();
  return (
    <>
      {!loggedIn &&
        <LoginPage />}
      {loggedIn &&
        <ChatPage />}
    </>
  )
}

export default App
