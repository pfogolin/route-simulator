using System.IO;
using Xunit;
using route_simulator.infra.repository;

namespace route_simulator.test
{
    public class IntegrationRepositoryTest
    {
        [Fact]
        public void empty_file_must_return_empty_list()
        {
            var inputRep = new RouteRepository("../../../stdin/no-content.csv");
            var result = inputRep.Get();
            Assert.True(result.Count == 0);
        }

        [Fact]
        public void invalid_source_must_throw_exception()
        {
            var inputRep = new RouteRepository("invalid-source");
            FileNotFoundException ex = Assert.Throws<FileNotFoundException>(() => inputRep.Get());
            Assert.Equal("stdin-not-found", ex.Message);
        }

        [Fact]
        public void input_route_must_remove_escaped_rows()
        {
            var inputRep = new RouteRepository("../../../stdin/escape-rows.csv");
            var result = inputRep.Get();

            Assert.True(result.Count == 5);
            Assert.True(result.Exists(x => x.AirportName.Contains("GRU")));
            Assert.True(result.Exists(x => x.AirportName.Contains("BRC")));
            Assert.True(result.Exists(x => x.AirportName.Contains("SCL")));
            Assert.True(result.Exists(x => x.AirportName.Contains("ORL")));
            Assert.True(result.Exists(x => x.AirportName.Contains("CDG")));
        }

        [Fact]
        public void input_sample_route_must_have_connections()
        {
            var inputRep = new RouteRepository("../../../stdin/input-routes_sample.csv");
            var result = inputRep.Get();

            Assert.True(result.Count == 2);
            Assert.True(result.Exists(x => x.AirportName.Equals("GRU")));
            Assert.True(result.Exists(x => x.AirportName.Equals("BRC")));
            Assert.True(result.Find(x => x.AirportName.Equals("GRU")).Connections.Exists(x => x.Airport.AirportName.Equals("BRC")));
            Assert.True(result.Find(x => x.AirportName.Equals("GRU")).Connections.Find(x => x.Airport.AirportName.Equals("BRC")).Price.Equals(10));
        }

        [Fact]
        public void new_route_must_be_persisted_in_file()
        {
            var newFile = "../../../stdin/new- route.csv";

            try
            {
                File.Create(newFile).Close();
                var inputRep = new RouteRepository(newFile);
                var newRoute = inputRep.Save(new infra.dto.RouteData("GIG", "LAG", 150));

                Assert.True(newRoute.OriginAirport.Equals("GIG"));
                Assert.True(newRoute.DestinationAirport.Equals("LAG"));
                Assert.True(newRoute.FlightPrice.Equals(150));

                var result = inputRep.Get();
                Assert.True(result.Count == 2);
                Assert.True(result.Exists(x => x.AirportName.Equals("GIG")));
                Assert.True(result.Exists(x => x.AirportName.Equals("LAG")));
                Assert.True(result.Find(x => x.AirportName.Equals("GIG")).Connections.Exists(x => x.Airport.AirportName.Equals("LAG")));
                Assert.True(result.Find(x => x.AirportName.Equals("GIG")).Connections.Find(x => x.Airport.AirportName.Equals("LAG")).Price == 150);
            }
            finally { if (File.Exists(newFile)) File.Delete(newFile); }

        }
    }
}

