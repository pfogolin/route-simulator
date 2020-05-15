using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using route_simulator.domain.entity;
using route_simulator.domain.repository;
using route_simulator.infra.dto;
using route_simulator.services;
using route_simulator.webapi.ViewModel;

namespace route_simulator.webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RouteController : ControllerBase
    {
        private readonly IRouteRepository _repository;

        public RouteController(IRouteRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("{origin}/{destination}")]
        public IActionResult GetBestRoute([FromRoute] string origin, string destination)
        {
            var routes = _repository.Get();

            var bestRoute = RouteService.BestRouteProcess(routes.Find(q => q.AirportName == origin), routes.Find(q => q.AirportName == destination));
            var result = new RouteVM() { RoutePathDescription = bestRoute.RoutePathDescription, TotalCost = bestRoute.TotalCost };
            return Ok(JsonSerializer.Serialize(result));
        }

        [HttpPost]
        public IActionResult SaveRoute([FromBody] RouteData route)
        {
            var result = _repository.Save(route);
            return Created(string.Empty, JsonSerializer.Serialize(result));
        }
    }
}