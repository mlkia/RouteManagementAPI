using RouteManagementTutorial.Entities;
using RouteManagementTutorial.Services;
using Microsoft.AspNetCore.Mvc;

namespace RouteManagementTutorial.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DriversController : ControllerBase
    {
        private readonly DriversService _driversService;

        public DriversController(DriversService driversService)
        {
            _driversService = driversService;
        }

        [HttpGet]
        public async Task<List<Driver>> Get() =>
            await _driversService.GetAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Driver>> Get(string id) 
            {
            var driver = await _driversService.GetAsync(id);
                if (driver is null)
                {
                    return NotFound();
                }
                return driver;
            }
            

        [HttpPost]
        public async Task<IActionResult> Post(Driver newDriver)
        {
            if (newDriver.Name == "") 
            { 
                return NotFound();
            }

            await _driversService.CreateAsync(newDriver);
            return CreatedAtAction(nameof(Get), new { id = newDriver.Id }, newDriver);
        }
           


        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Updat(string id, Driver updatedDriver)
        {
            var driver = await _driversService.GetAsync(id);

            if (driver is null)
            {
                return NotFound();
            }

            updatedDriver.Id = driver.Id;

            await _driversService.UpdateAsync(id, updatedDriver);

            return NoContent();
        }
            

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
               var driver = await _driversService.GetAsync(id);

            if (driver is null)
            {
                return NotFound();
            }

            await _driversService.DeleteAsync(id);

            return NoContent();
        }
            
    }
}
