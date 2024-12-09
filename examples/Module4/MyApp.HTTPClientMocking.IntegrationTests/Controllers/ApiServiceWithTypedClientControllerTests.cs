using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MyApp.HTTPClientMocking.Models;
using MyApp.HTTPClientMocking.Services;
using RichardSzalay.MockHttp;
using System.Net;
using System.Text.Json;

namespace MyApp.HTTPClientMocking.IntegrationTests.Controllers
{
    public class ApiServiceWithTypedClientControllerTests
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
        /// Mocking Typed HttpClient
        ///
        /// A Typed HttpClient is a custom class that is injected with HttpClient by ASP.NET Core.
        /// To mock a Typed HttpClient, you mock the HttpClient instance provided to the typed client
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task ShouldReturnMockedDataFromNamedHttpClient()
        {
            // Arrange
            // Create a mock HttpMessageHandler
            var mockHttp = new MockHttpMessageHandler();

            // Mocked response.
            var apiServiceMockResponse = new ApiServiceResponse { Method = "GET", Status = "Method Not Allowed" };

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
                    services.AddHttpClient<IApiServiceWithTypedClient, ApiServiceWithTypedClient>()
                            .ConfigurePrimaryHttpMessageHandler(() => mockHttp);
                });
            });

            // Create a mocked client for testing
            var client = factoryWithMock.CreateClient();

            // Act
            // Use the mocked client for testing
            var response = await client.GetAsync("/ApiServiceWithTypedClient");

            // Assert
            // Read the response content string
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
