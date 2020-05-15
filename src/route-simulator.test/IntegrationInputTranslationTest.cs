using System.IO;
using Xunit;
using route_simulator.infra.repository;

namespace route_simulator.test
{
    public class IntegrationInputTranslationTest
    {
        [Fact]
        public void empty_route_string_must_be_null()
        {
            string input = string.Empty;
            var interpreter = new RouteInput();

            Assert.Null(interpreter.Translate(input));
        }

        [Fact]
        public void route_translation_missing_property_must_be_null()
        {
            string input = "GRU,CDG";
            var interpreter = new RouteInput();
            Assert.Null(interpreter.Translate(input));
        }

        [Fact]
        public void route_translation_additional_property_must_be_initialized()
        {
            string input = "GRU,CDG,10";
            var interpreter = new RouteInput();
            Assert.NotNull(interpreter.Translate(input));
        }

        [Fact]
        public void route_translation_invalid_format_must_be_null()
        {
            string input = "asdfasdf8823djdDD33--";
            var interpreter = new RouteInput();
            Assert.Null(interpreter.Translate(input));
        }
    }
}
