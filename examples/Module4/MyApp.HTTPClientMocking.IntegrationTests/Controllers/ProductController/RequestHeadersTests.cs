using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace MyApp.HTTPClientMocking.IntegrationTests.Controllers.ProductController;

public class RequestHeadersTests
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
    /// Testing Request Headers.
    ///
    /// Ensure that required headers are present and processed correctly,
    /// and responses vary as expected based on header values
    /// </summary>
    /// <returns></returns>
    [Test]
    public async Task ShouldGetProductsReturnsDataWithValidAuthorizationHeader()
    {
        // Arrange
        var client = factory.Services.GetRequiredService<IHttpClientFactory>().CreateClient("dummyjson_client");

        // Act
        var response = await client.GetAsync($"/test");

        // Assert
        response.EnsureSuccessStatusCode();
        var headers = response.RequestMessage?.Headers;
        Assert.That(headers, Is.Not.Null);

        // Check the custom X-Custom-Header header is correctly set in the HttpClient
        Assert.That(headers.Contains("X-Custom-Header"), Is.True);
        Assert.That(headers.GetValues("X-Custom-Header").First(), Is.EqualTo("CustomValue"));

        // Check the Authorization header is correctly set in the HttpClient
        Assert.That(headers.Authorization, Is.Not.Null);
        Assert.That(headers.Authorization.Scheme, Is.EqualTo("Bearer"));
        Assert.That(headers.Authorization.Parameter, Is.EqualTo("some_access_token"));
    }
}
