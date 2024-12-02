using Hl7.Fhir.Rest;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MyApp.EnvironmentConfigForTesting.IntegrationTests.Utils;
using MyApp.EnvironmentConfigForTesting.Options;
using MyApp.EnvironmentConfigForTesting.Services;
using System.Net.Http.Headers;
using System.Reflection;

namespace MyApp.EnvironmentConfigForTesting.IntegrationTests.Controllers;

public class FhirControllerTests
{
    private WebApplicationFactory<Program>? _factory;
    private HttpClient? _client;

    [SetUp]
    public void SetUp()
    {
    }

    [TearDown]
    public void TearDown()
    {
        // Clean up resources
        _client?.Dispose();
        _factory?.Dispose();
    }

    /// <summary>
    /// // Define the Testing Environment to use the custom appSettings for Tests.
    /// </summary>
    /// <returns></returns>
    [Test]
    public async Task ShouldConfiguringTestEnvironment()
    {
        // Arrange
        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                // appsettings.Testing.json
                builder.UseEnvironment("Testing");

            });
        _client = _factory.CreateClient();

        // Act
        var response = await _client.GetAsync("Fhir/GetPatients?count=1");

        // Assert
        response.EnsureSuccessStatusCode();
    }

    /// <summary>
    /// Overriding App Settings for Integration Tests.
    /// Use ConfigureAppConfiguration: Modify or replace configuration sources
    /// </summary>
    /// <returns></returns>
    [Test]
    public async Task ShouldOverrideAppSettingsValues()
    {
        // Arrange
        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context, config) =>
                {
                    config.AddInMemoryCollection(new List<KeyValuePair<string, string?>>()
                    {
                        // Local FHIR data store.
                        //new KeyValuePair<string, string?>("FhirClientOptions:BaseUrl", "http://localhost:8080/fhir/"),
                        new KeyValuePair<string, string?>("Logging:LogLevel:Default", "Debug"),
                    });
                });
            });
        _client = _factory.CreateClient();

        // Act
        var response = await _client.GetAsync("Fhir/GetPatients?count=1");

        // Assert
        response.EnsureSuccessStatusCode();
    }

    /// <summary>
    /// Override Configuration file with appsettings.TestProject.json.
    /// Use ConfigureServices to customize dependency Injection with new appsettings for testing.
    /// </summary>
    /// <returns></returns>
    [Test]
    public async Task ShouldOverrideSpecificAppSettingsDynamically()
    {
        // Arrange
        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration(config =>
                {
                    // Clear existing configuration
                    config.Sources.Clear();

                    // Load base appsettings
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

                    // Get the path to the test project's output directory
                    string? testProjectPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                    // Load project-specific test settings
                    config.AddJsonFile(Path.Combine(testProjectPath!, "appsettings.TestProject.json"), optional: false, reloadOnChange: true);
                });

                builder.ConfigureServices(services =>
                {
                    // Customizing Dependency Injection for Testing

                    // Remove FhirService
                    var descriptor = services.Single(d => d.ServiceType == typeof(IFhirService));
                    services.Remove(descriptor);

                    // Add new FhirService with customized FhirClient
                    services.AddScoped<IFhirService, FhirService>((serviceProvider) =>
                    {
                        // Custom FhirClient with authorization for Tests
                        var fhirClientOptions = serviceProvider.GetRequiredService<IOptions<FhirClientOptions>>().Value;
                        var fhirClient = new FhirClient(fhirClientOptions.BaseUrl);
                        if (fhirClient.RequestHeaders != null)
                        {
                            fhirClient.RequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/yaml"));
                            fhirClient.RequestHeaders.Authorization = AuthorizationUtils.CreateBasicAuth(fhirClientOptions?.Authorization?.Id, fhirClientOptions?.Authorization?.Secret);
                        }
                        return new FhirService(fhirClient);
                    });
                });
            });
        _client = _factory.CreateClient();

        // Act
        var response = await _client.GetAsync("Fhir/GetPatients?count=1");

        // Assert
        response.EnsureSuccessStatusCode();
    }
}

