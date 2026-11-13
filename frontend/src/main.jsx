import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import App from './App.jsx'
import { AuthProvider } from './contexts/AuthContext.jsx'
import { HubProvider } from './contexts/HubContext.jsx'
import 'bootstrap/dist/css/bootstrap.min.css';
import './styles/utilities.css';

createRoot(document.getElementById('root')).render(
  <StrictMode>
    <AuthProvider>
      <HubProvider>
        <App />
      </HubProvider>
    </AuthProvider>
  </StrictMode>,
)
