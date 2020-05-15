using route_simulator.infra.dto;

namespace route_simulator.infra.repository
{
    public interface IInputRouteTranslate
    {
        RouteData Translate(string input);
    }

    public sealed class RouteInput : IInputRouteTranslate
    {
        private const int Origin = 0;
        private const int Destination = 1;
        private const int Price = 2;

        public RouteData Translate(string input)
        {
            try
            {
                var values = input.Split(',');
                var route = new RouteData(values[Origin], values[Destination], int.Parse(values[Price]));

                return route;
            }
            catch { return null; }
        }
    }
}
