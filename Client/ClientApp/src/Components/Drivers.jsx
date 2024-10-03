import { useState, useEffect } from 'react';

function Drivers() {
  const [drivers, setDrivers] = useState([]);

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
    </div>
  );
}

export default Drivers;
