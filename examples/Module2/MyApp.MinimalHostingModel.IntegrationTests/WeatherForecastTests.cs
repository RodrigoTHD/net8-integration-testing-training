using Microsoft.AspNetCore.Mvc.Testing;

namespace MyApp.MinimalHostingModel.IntegrationTests
{
    public class WeatherForecastTests
    {
        private WebApplicationFactory<Program> _factory;
        private HttpClient _client;

        [SetUp]
        public void SetUp()
        {
            // Initialize the WebApplicationFactory
            _factory = new WebApplicationFactory<Program>();

            // Create an HttpClient to send requests to the in-memory server
            _client = _factory.CreateClient();
        }

        [TearDown]
        public void TearDown()
        {
            // Clean up resources
            _client.Dispose();
            _factory.Dispose();
        }

        [Test]
        public async Task ShouldReturnsExpectedValuesWithWebApplicationFactory()
        {
            // Act: Send a GET request to the API endpoint
            var response = await _client.GetAsync("/WeatherForecast");

            // Assert: Ensure the response status is 200 OK
            response.EnsureSuccessStatusCode();
        }
    }
}