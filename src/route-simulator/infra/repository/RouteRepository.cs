using System;
using System.IO;
using System.Collections.Generic;
using route_simulator.domain.entity;
using route_simulator.domain.repository;
using route_simulator.infra.dto;
using System.Text;

namespace route_simulator.infra.repository
{
    public sealed class RouteRepository : IRouteRepository
    {
        private readonly string _sourceFile;

        public RouteRepository(string sourceFile)
        {
            _sourceFile = sourceFile;
        }

        public List<Route> Get()
        {
            if (!File.Exists(_sourceFile)) throw new FileNotFoundException("stdin-not-found");

            var routes = new List<Route>();

            using (var sr = new StreamReader(_sourceFile))
            {
                while (sr.Peek() >= 0)
                {
                    var lineRead = sr.ReadLine();
                    var input = Handle(lineRead);

                    if (input is null) continue;

                    var originAirport = routes.Find(q => q.AirportName.Equals(input.OriginAirport));
                    var destinationAirport = routes.Find(q => q.AirportName.Equals(input.DestinationAirport));

                    if (originAirport == null)
                    {
                        originAirport = new Route(input.OriginAirport);
                        routes.Add(originAirport);
                    }

                    if (destinationAirport == null)
                    {
                        destinationAirport = new Route(input.DestinationAirport);
                        routes.Add(destinationAirport);
                    }

                    originAirport.ConnectTo(destinationAirport, input.FlightPrice);
                }
            }
            return routes;
        }

        RouteData Handle(string input)
        {
            var translateRoutes = new List<IInputRouteTranslate>() { new RouteInput() };

            foreach (var item in translateRoutes)
            {
                var route = item.Translate(input);
                if (route != null) return route;
            }
            return null;
        }

        public RouteData Save(RouteData route)
        {
            if (!File.Exists(_sourceFile)) throw new FileNotFoundException("stdin-not-found");

            StringBuilder fileContent = new StringBuilder();

            using (var sr = new StreamReader(_sourceFile))
            {
                while (sr.Peek() >= 0)
                {
                    var lineRead = sr.ReadLine();
                    fileContent.AppendLine(lineRead);
                }
            }

            fileContent.AppendLine($"{ route.OriginAirport },{ route.DestinationAirport },{ route.FlightPrice }");
            File.WriteAllText(_sourceFile, fileContent.ToString());

            return route;
        }
    }
}
