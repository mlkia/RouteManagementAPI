import { useState, useEffect } from 'react';

function Drivers() {
  const [drivers, setDrivers] = useState([]);
  const [name, setName] = useState("");
  const [personNumber, setPersonNumber] = useState("");
  const [phoneNumber, setPhoneNumber] = useState("");
  const [email, setEmail] = useState("");
  const [licenseType, setLicenseType] = useState("");


  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await fetch('https://localhost:7116/api/Drivers');
        if (response.ok) {
          const data = await response.json();
          setDrivers(data);
        } else {
          console.error('Failed to fetch drivers');
        }
      } catch (error) {
        console.error('Error:', error);
      }
    };

    fetchData();
  }, []);

  const buildDriver = () => {
    return {
      "id": "",
      "name": name,
      "personNumber": personNumber,
      "phoneNumber": phoneNumber,
      "email": email,
      "licenseType": licenseType
    };
  };

  const submitDriver = async () => {
    
    const newDriver = buildDriver();
    const result = await fetch ('https://localhost:7116/api/Drivers', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(newDriver)
    })

    const resultInJson = await result.json
    console.log(resultInJson)
    
  }
  return (
    <div>
      <h4>Drivers</h4>
      
      <ul>
        {drivers.map((driver) => (
          <li key={driver.id}>
            {driver.name} - {driver.email}
          </li>
        ))}
      </ul>
      <input
            type="text"
            placeholder="Driver Name"
            value={name}
            onChange={(e) => setName(e.target.value)}
            required
          ></input>
            <br/>
           <input
            type="text"
            placeholder="National Number"
            value={personNumber}
            onChange={(e) => setPersonNumber(e.target.value)}
            required
          ></input>
            <br/>
           <input
            type="text"
            placeholder="Phone Number"
            value={phoneNumber}
            onChange={(e) => setPhoneNumber(e.target.value)}
            required
          ></input>
            <br/>
           <input
            type="text"
            placeholder="Email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          ></input>
            <br/>
           <input
            type="text"
            placeholder="License Type"
            value={licenseType}
            onChange={(e) => setLicenseType(e.target.value)}
            required
          ></input>
            <br/>
            <br/>
          <button onClick={submitDriver}>Submit a new Driver</button>

    </div>
  );
}

export default Drivers;
