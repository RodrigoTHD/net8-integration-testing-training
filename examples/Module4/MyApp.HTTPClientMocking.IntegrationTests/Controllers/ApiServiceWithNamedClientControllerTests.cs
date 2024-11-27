using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MyApp.HTTPClientMocking.Models;
using RichardSzalay.MockHttp;
using System.Net;
using System.Text.Json;

namespace MyApp.HTTPClientMocking.IntegrationTests.Controllers
{
    public class ApiServiceWithNamedClientControllerTests
    {
        private WebApplicationFactory<Program> factory;

        [SetUp]
        public void Setup()
        {
            // Initialize the WebApplicationFactory
            factory = new WebApplicationFactory<Program>();
        }

        [TearDown]
        public void TearDown()
        {
            // Clean up resources
            factory?.Dispose();
        }

        /// <summary>
        /// Mocking Named HttpClient.
        ///
        /// A Named HttpClient is one where you specify a name when registering the HttpClient in the DI container.
        /// To mock a Named HttpClient, you replace the client with a mock version using a WebApplicationFactory test setup.
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
            var factoryWithMock = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Register the named HttpClient mock.
                    services.AddHttpClient("ApiService_ClientName")
                            .ConfigurePrimaryHttpMessageHandler(() => mockHttp);
                });
            });

            var client = factoryWithMock.CreateClient();

            // Act
            // Use the mocked client for testing
            var response = await client.GetAsync("/ApiServiceWithNamedClient");

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
