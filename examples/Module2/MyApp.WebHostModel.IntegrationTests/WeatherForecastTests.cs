using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace MyApp.WebHostModel.IntegrationTests;

public class WeatherForecastTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task ShouldCreateTestServer()
    {
        // Arrange
        var builder = new WebHostBuilder()
            .UseStartup<Startup>();
        var server = new TestServer(builder);
        var client = server.CreateClient();

        // Act
        var response = await client.GetAsync("/WeatherForecast");

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Test]
    public async Task ShouldUseTestServer()
    {
        // Arrange
        var host = new WebHostBuilder()
            .UseTestServer()
            .UseStartup<Startup>()
            .Start();

        // Act
        var response = await host.GetTestClient().GetAsync("/WeatherForecast");

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Test]
    public async Task ShouldCreateTestServerWithCustomizations()
    {
        // Arrange
        // Create the TestServer with WebHostBuilder
        var webHostBuilder = new WebHostBuilder()
            .ConfigureServices(services =>
            {
                // Add required services (same as in Program.cs)
                //TODO: create example with mock
            })
            .UseStartup<Startup>();
        // Create TestServer with WebHostBuilder
        var testServer = new TestServer(webHostBuilder);
        var client = testServer.CreateClient();

        // Act
        var response = await client.GetAsync("/WeatherForecast");

        // Assert
        response.EnsureSuccessStatusCode();
    }
}
