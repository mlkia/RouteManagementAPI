using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RouteManagementTutorial.Services;
using Entities;
using Route = Entities.Route; //This is to avoid confusion with the Route class in the 'Microsoft.AspNetCore.Routing.Route'

namespace RouteManagementTutorial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RouteController : ControllerBase
    {
        private readonly RouteService _routeService;
        public RouteController(RouteService routeService)
        {
            _routeService = routeService;
        }

        [HttpGet]
        public async Task<List<Route>> Get() =>
            await _routeService.GetAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Route>> Get(string id)
        {
            var route = await _routeService.GetAsync(id);
            if (route is null)
            {
                return NotFound("Route is not found");
            }
            return route;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Route newRoute)
        {
            await _routeService.CreateAsync(newRoute);
            return Ok("Success");
        }
    }
}
