import { useState } from 'react'
import { BrowserRouter as Router, NavLink, Routes, Route, Navigate } from 'react-router-dom'
import reactLogo from './assets/react.svg'
import viteLogo from '/vite.svg'
import Drivers from './Components/Drivers'
import './App.css'
import Login from './Components/Login'

function App() {
  

  return (
    <Router>
    <div>
      <Routes>
      <Route path="/" element={<Login />} />
      </Routes>
    </div>
    </Router>
  )
}

export default App
