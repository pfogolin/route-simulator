using System.Collections.Generic;
using route_simulator.domain.entity;
using route_simulator.infra.dto;

namespace route_simulator.domain.repository
{
    public interface IRouteRepository
    {
        List<Route> Get();

        RouteData Save(RouteData route);
    }
}
