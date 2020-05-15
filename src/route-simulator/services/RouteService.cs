using System.Linq;
using System.Collections.Generic;
using route_simulator.domain.entity;

namespace route_simulator.services
{
    public class RouteService 
    {
        private class RouteCost
        {
            public Route From { get; }

            public int Price { get; }

            public RouteCost(Route from, int price)
            {
                From = from;
                Price = price;
            }
        }

        class VisitingData
        {
            readonly List<Route> _visitedRoutes = new List<Route>();

            readonly Dictionary<Route, RouteCost> _prices = new Dictionary<Route, RouteCost>();

            readonly List<Route> _scheduledRoutes = new List<Route>();

            public void RegisterVisitTo(Route route)
            {
                if (!_visitedRoutes.Contains(route))
                    _visitedRoutes.Add((route));
            }

            public bool WasVisited(Route route)
            {
                return _visitedRoutes.Contains(route);
            }

            public void UpdateFlightPrice(Route route, RouteCost newPrice)
            {
                if (!_prices.ContainsKey(route))
                {
                    _prices.Add(route, newPrice);
                }
                else
                {
                    _prices[route] = newPrice;
                }
            }

            public RouteCost FlightPrice(Route route)
            {
                RouteCost result;

                if (!_prices.ContainsKey(route))
                {
                    result = new RouteCost(null, int.MaxValue);
                    _prices.Add(route, result);
                }
                else
                {
                    result = _prices[route];
                }

                return result;
            }

            public void ScheduleVisitTo(Route route)
            {
                _scheduledRoutes.Add(route);
            }

            public bool HasScheduledVisits => _scheduledRoutes.Count > 0;

            public Route GetRouteToVisit()
            {
                var ordered = from n in _scheduledRoutes
                              orderby FlightPrice(n).Price
                              select n;

                var result = ordered.First();
                _scheduledRoutes.Remove(result);
                return result;
            }

            public bool HasComputedPathToOrigin(Route route)
            {
                return FlightPrice(route).From != null;
            }

            public IEnumerable<Route> GetEntireRoute(Route route)
            {
                var predecessor = route;

                while (predecessor != null)
                {
                    yield return predecessor;
                    predecessor = FlightPrice(predecessor).From;
                }
            }
        }

        public static RouteResult BestRouteProcess(Route origin, Route destination)
        {
            var control = new VisitingData();

            control.UpdateFlightPrice(origin, new RouteCost(null, 0));
            control.ScheduleVisitTo(origin);

            while (control.HasScheduledVisits)
            {
                var visitingRoute = control.GetRouteToVisit();
                var visitingFlightPrice = control.FlightPrice(visitingRoute);
                control.RegisterVisitTo(visitingRoute);

                foreach (var connectionInfo in visitingRoute.Connections)
                {
                    if (!control.WasVisited(connectionInfo.Airport))
                    {
                        control.ScheduleVisitTo(connectionInfo.Airport);
                    }

                    var connectionCost = control.FlightPrice(connectionInfo.Airport);

                    var probablePrice = (visitingFlightPrice.Price + connectionInfo.Price);
                    if (connectionCost.Price > probablePrice)
                    {
                        control.UpdateFlightPrice(connectionInfo.Airport, new RouteCost(visitingRoute, probablePrice));
                    }
                }
            }

            RouteResult result = new RouteResult();

            var cost = control.FlightPrice(destination);
            result.TotalCost = cost.Price;
            result.Routes = cost.From == null ? null : control.GetEntireRoute(destination).Reverse().ToList();

            return result;
        }
    }
}
