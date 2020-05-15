using System.IO;
using Xunit;
using route_simulator.infra.repository;
using route_simulator.services;
using Moq;
using route_simulator.domain.repository;
using route_simulator.domain.entity;
using System.Collections.Generic;

namespace route_simulator.test
{
    public class ServiceRouteTest
    {
        [Fact]
        public void process_valid_route_single_option()
        {
            var route1 = new Route("GRU");
            var route2 = new Route("CGD");
            route1.ConnectTo(route2, 10);

            var routes = new List<Route>();
            routes.Add(route1);
            routes.Add(route2);

            var result = RouteService.BestRouteProcess(route1, route2);

            Assert.True(result.RoutePathDescription.Equals("GRU-CGD"));
            Assert.True(result.TotalCost.Equals(10));
        }

        [Fact]
        public void process_valid_route_multiple_options_many_connections()
        {
            var route1 = new Route("GRU");
            var route2 = new Route("BRC");
            var route3 = new Route("CGD");
            
            route1.ConnectTo(route2, 100);
            route1.ConnectTo(route3, 10);
            route3.ConnectTo(route2, 50);            

            var routes = new List<Route>();
            routes.Add(route1);
            routes.Add(route2);
            routes.Add(route3);

            var result = RouteService.BestRouteProcess(route1, route2);

            Assert.True(result.RoutePathDescription.Equals("GRU-CGD-BRC"));
            Assert.True(result.TotalCost.Equals(60));
            Assert.True(result.Routes.Count == 3);
        }

        [Fact]
        public void process_valid_route_multiple_options_one_connection()
        {
            var route1 = new Route("GRU");
            var route2 = new Route("BRC");
            var route3 = new Route("CGD");

            route1.ConnectTo(route2, 50);
            route1.ConnectTo(route3, 10);
            route3.ConnectTo(route2, 50);

            var routes = new List<Route>();
            routes.Add(route1);
            routes.Add(route2);
            routes.Add(route3);

            var result = RouteService.BestRouteProcess(route1, route2);

            Assert.True(result.RoutePathDescription.Equals("GRU-BRC"));
            Assert.True(result.TotalCost.Equals(50));
            Assert.True(result.Routes.Count == 2);
        }

        [Fact]
        public void process_invalid_route()
        {
            var route1 = new Route("GRU");
            var route2 = new Route("BRC");            
            route1.ConnectTo(route2, 10);

            var route3 = new Route("SCL");
            var route4 = new Route("ORL");
            route3.ConnectTo(route4, 20);

            var routes = new List<Route>();
            routes.Add(route1);
            routes.Add(route2);
            routes.Add(route3);
            routes.Add(route4);

            var result = RouteService.BestRouteProcess(route1, route4);

            Assert.True(result.RoutePathDescription.Equals("no route available"));
            Assert.True(result.Routes == null);
        }
    }
}