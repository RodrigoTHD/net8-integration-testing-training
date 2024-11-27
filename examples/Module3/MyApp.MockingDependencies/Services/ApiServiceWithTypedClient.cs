using System.Text.Json;

namespace MyApp.MockingDependencies.Services;

public class ApiServiceWithTypedClient : IApiServiceWithTypedClient
{
    private readonly HttpClient httpClient;

    // The HttpClient is injected by the DI container
    public ApiServiceWithTypedClient(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<T?> GetDataAsync<T>(string route) where T : new()
    {
        HttpResponseMessage response = await httpClient.GetAsync($"{route.TrimStart('/')}");
        response.EnsureSuccessStatusCode();
        string json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
}
