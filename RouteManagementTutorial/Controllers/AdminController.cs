using RouteManagementTutorial.Services;
using Microsoft.AspNetCore.Mvc;
using Entities;
using Microsoft.AspNetCore.Authorization;


namespace RouteManagementTutorial.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly AdminService _adminService;

        public AdminController(AdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet]
        public async Task<List<Admin>> Get() =>
            await _adminService.GetAsync();


        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Admin>> Get(string id) 
            {

            var admin = await _adminService.GetAsync(id);
                if (admin is null)
                {
                    return NotFound("User is not found");
                }
                return admin;
            }

        [AllowAnonymous]
        [HttpPost("login")]

        public IActionResult Login([FromBody] Admin admin)
        {

              var token = _adminService.Authenticate(admin.Email, admin.Password);
            if (token == null)
                return Unauthorized();

            return Ok(new { Token = token });
        }

        [HttpPost]
        public async Task<IActionResult> Post(Admin newAdmin)
        {

            await _adminService.CreateAsync(newAdmin);
            
            return Ok("Success");
        }
    }
}
