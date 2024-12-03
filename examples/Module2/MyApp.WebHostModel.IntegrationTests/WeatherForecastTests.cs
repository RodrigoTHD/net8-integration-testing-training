using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace MyApp.WebHostModel.IntegrationTests;

public class WeatherForecastTests
{
    /// <summary>
    /// Configuring in-memory test server with WebHostBuilder and TestServer.
    /// 
    /// </summary>
    /// <returns></returns>
    [Test]
    public async Task ShouldCreateTestServer()
    {
        // Arrange
        // Create the WebHostBuilder
        var builder = new WebHostBuilder()
            .UseStartup<Startup>();

        // Create the TestServer for with the build
        var server = new TestServer(builder);

        // Create the test httpClient to perform requests to the TestServer.
        var client = server.CreateClient();

        // Act
        // Send HTTP requests to the in-memory server.
        var response = await client.GetAsync("/WeatherForecast");

        // Assert
        // Verify the response status is 200 OK
        Assert.True(response.IsSuccessStatusCode);
    }

    /// <summary>
    /// Configuring in-memory test server with WebHostBuilder and
    /// TestServer and TestClient from extensions.
    /// 
    /// </summary>
    /// <returns></returns>
    [Test]
    public async Task ShouldUseTestServer()
    {
        // Arrange
        // Create the WebHostBuilder with TestServer
        var builder = new WebHostBuilder()
            .UseTestServer()
            .UseStartup<Startup>()
            .Start();

        // Act
        // Get TestClient and send HTTP requests to the in-memory server.
        var response = await builder.GetTestClient().GetAsync("/WeatherForecast");

        // Assert
        // Verify the response status is 200 OK
        Assert.True(response.IsSuccessStatusCode);
    }

    /// <summary>
    /// Configuring in-memory test server with WebHostBuilder and
    /// customizations with ConfigureServices.
    /// 
    /// </summary>
    /// <returns></returns>
    [Test]
    public async Task ShouldCreateTestServerWithCustomizations()
    {
        // Arrange
        // Create the TestServer with WebHostBuilder
        var builder = new WebHostBuilder()
            .ConfigureServices(services =>
            {
                // Add mock services here (same as in Program.cs)
                // services.AddScoped(_ => mockServiceExample);
            })
            .UseTestServer()
            .UseStartup<Startup>()
            .Start();

        // Act
        // Get TestClient and send HTTP requests to the in-memory server.
        var response = await builder.GetTestClient().GetAsync("/WeatherForecast");

        // Assert
        // Verify the response status is 200 OK
        Assert.True(response.IsSuccessStatusCode);
    }
}
