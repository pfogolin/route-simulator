using System;
using route_simulator.services;

namespace route_simulator.consoleapp
{
    class Program
    {
        private const int FileName = 0;
        private const int Origin = 0;
        private const int Destination = 1;

        static void Main(string[] args)
        {
            if (args.Length == 0) throw new ArgumentNullException("missing-argument");
            
            var rep = new infra.repository.RouteRepository($"stdin/{args[FileName]}");
            var routes = rep.Get();

            while (true)
            {
                Console.Write("please enter the route: ");
                var input = Console.ReadLine();
                var routeToFind = input.Split('-');

                var origin = routes.Find(q => q.AirportName == routeToFind[Origin]);
                var destination = routes.Find(q => q.AirportName == routeToFind[Destination]);

                if (origin != null && destination != null)
                {
                    var result = RouteService.BestRouteProcess(origin, destination);
                    Console.Write($"best route: {result.BestRouteDescription}");
                }
                else Console.Write($"invalid input route");

                Console.WriteLine();
            }
        }
    }
}
