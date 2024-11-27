using System.Text.Json;

namespace MyApp.MockingDependencies.Services;

public class ApiServiceWithClientFactory : IApiServiceWithClientFactory
{
    private readonly IHttpClientFactory httpClientFactory;

    public ApiServiceWithClientFactory(IHttpClientFactory httpClientFactory)
    {
        this.httpClientFactory = httpClientFactory;
    }

    public async Task<T?> GetDataAsync<T>(string route) where T : new()
    {
        var httpClient = httpClientFactory.CreateClient("ApiServiceHttpClientName");
        HttpResponseMessage response = await httpClient.GetAsync($"{route.TrimStart('/')}");
        response.EnsureSuccessStatusCode();
        string json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
}
