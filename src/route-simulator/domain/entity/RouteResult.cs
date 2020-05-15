using System;
using System.Collections.Generic;

namespace route_simulator.domain.entity
{
    public class RouteResult
    {
        public List<Route> Routes { get; set; }

        public int TotalCost { get; set; }

        public string RoutePathDescription
        {
            get
            {
                var result = string.Empty;
                if (Routes != null) Routes.ForEach(x => result = $"{result}-{x.AirportName}");
                return string.IsNullOrEmpty(result) ? "no route available" : result.Remove(0, 1);
            }
        }

        public string BestRouteDescription => Routes == null ? RoutePathDescription : $"{RoutePathDescription} > ${TotalCost}";
    }
}
