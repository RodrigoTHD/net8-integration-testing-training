using MyApp.EnvironmentConfigForTesting.Models;

namespace MyApp.EnvironmentConfigForTesting.Options;

public class FhirClientOptions
{
    public string BaseUrl { get; set; } = string.Empty;

    public Authorization? Authorization { get; set; }
}
