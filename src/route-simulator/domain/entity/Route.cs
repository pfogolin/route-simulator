using System;
using System.Collections.Generic;

namespace route_simulator.domain.entity
{
    public class Route
    {
        public string AirportName { get; }

        public Route(string airportName)
        {
            AirportName = airportName;
        }

        readonly List<RouteInfo> _connections = new List<RouteInfo>();

        public List<RouteConnection> Connections
        {
            get
            {
                List<RouteConnection> connections = new List<RouteConnection>();

                foreach (var connection in _connections)
                {
                    connections.Add(new RouteConnection(
                            connection.Airport1 == this ? connection.Airport2 : connection.Airport1,
                            connection.Price));
                }

                return connections;
            }
        }

        private void Assign(RouteInfo connection)
        {
            _connections.Add(connection);
        }

        public void ConnectTo(Route destination, int price)
        {
            RouteInfo.Create(price, this, destination);
        }

        public struct RouteConnection
        {
            public Route Airport { get; }
            
            public int Price { get; }

            public RouteConnection(Route airport, int price)
            {
                Airport = airport;
                Price = price;
            }
        }

        public class RouteInfo
        {
            public int Price { get; }

            public Route Airport1 { get; }

            public Route Airport2 { get; }

            public RouteInfo(int price, Route airport1, Route airport2)
            {
                Price = price;
                Airport1 = airport1;
                airport1.Assign(this);
                Airport2 = airport2;
                airport2.Assign(this);
            }

            public static RouteInfo Create(int price, Route node1, Route node2)
            {
                return new RouteInfo(price, node1, node2);
            }
        }
    }
}
