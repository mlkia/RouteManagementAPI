import { BrowserRouter as Router, NavLink, Routes, Route, Navigate } from 'react-router-dom'
import Drivers from './Components/Drivers'
import './App.css'
import Login from './Components/Login/Login'
import DriverProfile from './Components/DriverProfile/DriverProfile'
import SignUpView from './Components/SignUp/SignUpView'

function App() {
  

  return (
    <Router>
    <div>
      <Routes>
      <Route path="/" element={<Login />} />
      <Route path="/signup" element={<SignUpView />} />
      <Route path="/profile" element={<DriverProfile />} />
      </Routes>
    </div>
    </Router>
  )
}

export default App
