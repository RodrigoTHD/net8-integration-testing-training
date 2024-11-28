using System.Text.Json;

namespace MyApp.HTTPClientMocking.Services;

public class ApiServiceWithNamedClient : IApiServiceWithNamedClient
{
    private readonly IHttpClientFactory httpClientFactory;

    public ApiServiceWithNamedClient(IHttpClientFactory httpClientFactory)
    {
        this.httpClientFactory = httpClientFactory;
    }

    public async Task<T?> GetDataAsync<T>(string route) where T : new()
    {
        var httpClient = httpClientFactory.CreateClient("dummyjson_client");
        HttpResponseMessage response = await httpClient.GetAsync($"{route.TrimStart('/')}");
        response.EnsureSuccessStatusCode();
        string json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
}
