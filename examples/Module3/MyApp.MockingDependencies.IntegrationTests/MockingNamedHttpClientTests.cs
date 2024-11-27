using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MyApp.MockingDependencies.Model;
using RichardSzalay.MockHttp;
using System.Net;
using System.Text.Json;

namespace MyApp.MockingDependencies.IntegrationTests
{
    public class MockingTypedHttpClientTests
    {
        private WebApplicationFactory<Program> _factory;

        [SetUp]
        public void Setup()
        {
            // Initialize the WebApplicationFactory
            _factory = new WebApplicationFactory<Program>();
        }

        [TearDown]
        public void TearDown()
        {
            // Clean up resources
            _factory?.Dispose();
        }

        /// <summary>
        /// Mocking Named HttpClient.
        ///
        /// A Named HttpClient is one where you specify a name when registering the HttpClient in the DI container.
        /// To mock a Named HttpClient, you replace the client with a mock version using a test setup.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task ShouldReturnMockedDataFromNamedHttpClient()
        {
            // Arrange
            // Create a mock HttpMessageHandler
            var mockHttp = new MockHttpMessageHandler();

            // Mocked response.
            var apiServiceMockResponse = new ApiServiceResponse { Method = "GET", Status = "Accepted" };

            // Mocked response serialized.
            string json = JsonSerializer.Serialize(apiServiceMockResponse);

            // Setup the mock response for a specific request
            mockHttp.When("https://dummyjson.com/test")
                .Respond(HttpStatusCode.OK, "application/json", json);

            // Use WebApplicationFactory to override HttpClient
            var factoryWithMock = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Register the named HttpClient mock.
                    services.AddHttpClient("ApiServiceHttpClientName")
                            .ConfigurePrimaryHttpMessageHandler(() => mockHttp);
                });
            });

            var client = factoryWithMock.CreateClient();

            // Act
            // Use the mocked client for testing
            var response = await client.GetAsync("/ApiServiceWithClientFactory");

            // Assert
            var jsonResponse = await response.Content.ReadAsStringAsync();
            ApiServiceResponse? apiServiceResponse = JsonSerializer.Deserialize<ApiServiceResponse>(
                jsonResponse,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Verify that the mock returns the expected response
            Assert.That(
                apiServiceResponse,
                Has.Property("Method").EqualTo(apiServiceMockResponse.Method) &
                Has.Property("Status").EqualTo(apiServiceMockResponse.Status));
        }
    }
}
