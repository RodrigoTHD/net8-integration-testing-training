using FakeItEasy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MyApp.HTTPClientMocking.IntegrationTests.Utils;
using MyApp.HTTPClientMocking.Models;
using MyApp.HTTPClientMocking.Services;
using System.Net;
using System.Text;
using System.Text.Json;

namespace MyApp.HTTPClientMocking.IntegrationTests.ManualMock;
public class HttpClientManualMockTest
{
    /// <summary>
    /// Manual mocking HTTPClient response.
    /// </summary>
    /// <returns></returns>
    [Test]
    public async Task ShouldReturnHttpClientMockResponse()
    {
        // Arrange
        // Create a mock IHttpMessageHandlerMock
        var fakeHandler = A.Fake<IHttpMessageHandlerMock>();


        // Create the ApiService response data
        var apiServiceMockResponse = new ApiServiceResponse
        {
            Method = "GET",
            Status = "ok"
        };

        // Mock the response in the fake handler
        A.CallTo(() => fakeHandler.SendAsync(A<HttpRequestMessage>.Ignored, A<CancellationToken>.Ignored))
        .Returns(new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonSerializer.Serialize(apiServiceMockResponse), Encoding.UTF8, "application/json")
        });

        // Create the HttpMessageHandler with mock
        var originalHandlerWithMock = new HttpMessageHandlerMock { Mock = fakeHandler };

        // Initialize the WebApplicationFactory
        var factory = new WebApplicationFactory<Program>();

        // Use WebApplicationFactory to override HttpClient
        var factoryWithMock = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddHttpClient<IApiServiceWithTypedClient, ApiServiceWithTypedClient>()
                            .ConfigurePrimaryHttpMessageHandler(() => originalHandlerWithMock);
            });
        });

        // Create a mocked client for testing
        var client = factoryWithMock.CreateClient();

        // Act
        var response = await client.GetAsync("/ApiServiceWithTypedClient");

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
