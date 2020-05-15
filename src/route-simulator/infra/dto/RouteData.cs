namespace route_simulator.infra.dto
{
    public class RouteData 
    {
        public RouteData()
        { 
        }

        public RouteData(string origin, string destination, int price)
        {
            OriginAirport = origin;
            DestinationAirport = destination;
            FlightPrice = price;
        }

        public string OriginAirport { get; set; }

        public string DestinationAirport { get; set; }

        public int FlightPrice { get; set; }
    }
}
