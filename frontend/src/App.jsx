import LoginPage from './pages/LoginPage/LoginPage'
import ChatPage from './pages/ChatPage/ChatPage'
import { UseAuth } from './contexts/AuthContext'

function App() {
  const { loggedIn } = UseAuth();
  return (
    <section
      className="2xl:container">
      <div className={loggedIn
        ? "app-container"
        : "d-flex justify-content-center align-items-center vh-100 bg-primary-subtle"
      }>
        {!loggedIn ? <LoginPage /> : <ChatPage />}
      </div>
    </section>
  )
}

export default App
