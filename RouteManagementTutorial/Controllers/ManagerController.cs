using RouteManagementTutorial.Services;
using Microsoft.AspNetCore.Mvc;
using Entities;
using Microsoft.AspNetCore.Authorization;


namespace RouteManagementTutorial.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ManagerController : ControllerBase
    {
        private readonly ManagerService _managerService;

        public ManagerController(ManagerService managerService)
        {
            _managerService = managerService;
        }

        [HttpGet]
        public async Task<List<Manager>> Get() =>
            await _managerService.GetAsync();


        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Manager>> Get(string id) 
            {

            var manager = await _managerService.GetAsync(id);
                if (manager is null)
                {
                    return NotFound("User is not found");
                }
                return manager;
            }

        [AllowAnonymous]
        [HttpPost("login")]

        public IActionResult Login([FromBody] Manager manager)
        {

              var token = _managerService.Authenticate(manager.Email, manager.Password);
            if (token == null)
                return Unauthorized();

            return Ok(new { Token = token });
        }

        [HttpPost]
        public async Task<IActionResult> Post(Manager newManager)
        {

            await _managerService.CreateAsync(newManager);
            
            return Ok("Success");
        }
    }
}
