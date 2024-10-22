import React, { useState } from "react";

function Login(){
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [driverProfile, setDriverProfile] = useState([]);
    const [error, setError] = useState(null);


    const handleSubmit = async (e) => {
        e.preventDefault();
        const credentials = {
            "email": email,
            "password": password,
        };

        try{
            const response = await fetch('https://localhost:7116/api/Drivers/login',{
             method: 'POST',
             headers: {
                'Content-Type': 'application/json'
             },
             body: JSON.stringify(credentials),
            });

            if(!response.ok) {
                throw new Error ('Login failed');
            }
            
            const data = await response.json();

             // Store the token in localStorage
             localStorage.setItem('token', data.token);

             console.log('Logged in successfully');
             // Redirect user to another page or update state

             const token = localStorage.getItem('token');

             const driverResponse = await fetch('https://localhost:7116/api/Drivers/Profile', {
                method: 'GET',
                headers: {
                  'Authorization': `Bearer ${token}`,
                  'Content-Type': 'application/json',
                },
              });
              
              const DriverData = await driverResponse.json();

              setDriverProfile(DriverData)

              console.log(DriverData);



        }   catch(error){
            setError(error.message);
        }
    };

    return (
<div>
      <h2>Login</h2>
      <form onSubmit={handleSubmit}>
        <input
          type="email"
          placeholder="Email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          required
        />
        <br />
        <input
          type="password"
          placeholder="Password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          required
        />
        <br />
        <button type="submit">Login</button>
      </form>
      {error && <p>{error}</p>}
    </div>
    );
}

export default Login;