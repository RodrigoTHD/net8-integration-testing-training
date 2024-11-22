using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

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
            .UseTestServer()
            .UseStartup<Startup>();
        var server = new TestServer(builder);
        var client = server.CreateClient();

        // Act
        var response = await client.GetAsync("/WeatherForecast");

        // Assert
        response.EnsureSuccessStatusCode();
    }

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
                services.AddControllers();
                services.AddEndpointsApiExplorer();
            })
            .Configure(app =>
            {
                // Replicate middleware and routing setup
                app.UseRouting();
                app.UseAuthorization();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
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
