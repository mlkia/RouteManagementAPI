﻿using RouteManagementTutorial.Entities;
using RouteManagementTutorial.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using RouteManagementTutorial.DTO;

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

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<List<Driver>> Get() =>
            await _driversService.GetAsync();


        [Authorize]
        [HttpGet("Profile")]
        public async Task<ActionResult<Driver>> GetOne() 
        {
            var loggedInEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(loggedInEmail))
            {
                return BadRequest();
            }

            var driver = await _driversService.GetByEmail(loggedInEmail);

            if (driver is null)
            {
                return NotFound("Driver is not found");
            }

            if (loggedInEmail != driver.Email)
            {
                return Forbid();
            }

            return driver;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserAuthenticate driver)
        {
            var token = _driversService.Authenticate(driver.Email, driver.Password, "Driver");

            if (token == null)
            {
                return Unauthorized();
            }

            return Ok(new { Token = token });
        }

        /// <summary>
        /// Creates a new driver.
        /// </summary>
        /// <param name="newDriver">The driver object to create.</param>
        /// <returns>The result of the creation operation.</returns>
        /// <response code="200">If the driver is successfully created.</response>
        /// <response code="400">If the creation fails.</response>
        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Post(Driver newDriver)
        {

            var result = await _driversService.CreateAsync(newDriver);

            if (!result.Success)
            {
                return BadRequest(newDriver);
            }
            
            return Created("https://localhost:7116/api/Drivers"+newDriver.Id, newDriver);
        }


        /// <summary>
        /// Updates an existing driver.
        /// </summary>
        /// <param name="id">The unique identifier of the driver (24 characters long).</param>
        /// <param name="updatedDriver">The updated driver object.</param>
        /// <returns>An IActionResult indicating the result of the update operation.</returns>
        /// <response code="204">If the driver is successfully updated.</response>
        /// <response code="404">If the driver is not found.</response>
        [Authorize(Roles = "Admin")]
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

        /// <summary>
        /// Deletes an existing driver.
        /// </summary>
        /// <param name="id">The unique identifier of the driver (24 characters long).</param>
        /// <returns>An IActionResult indicating the result of the delete operation.</returns>
        /// <response code="204">If the driver is successfully deleted.</response>
        /// <response code="404">If the driver is not found.</response>
        [Authorize(Roles = "Admin")]
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
