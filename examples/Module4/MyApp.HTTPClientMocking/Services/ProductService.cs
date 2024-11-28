using Microsoft.AspNetCore.WebUtilities;
using MyApp.HTTPClientMocking.Models;
using System.Text.Json;

namespace MyApp.HTTPClientMocking.Services;

public class ProductService : IProductService
{
    private readonly IHttpClientFactory httpClientFactory;

    public ProductService(IHttpClientFactory httpClientFactory)
    {
        this.httpClientFactory = httpClientFactory;
    }

    public async Task<Product?> GetProduct(string id)
    {
        var httpClient = httpClientFactory.CreateClient("dummyjson_client");
        var response = await httpClient.GetAsync($"/products/{id}");

        if (response.IsSuccessStatusCode)
        {
            string json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Product>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        return null;
    }

    public async Task<ProductsResponse?> GetProducts(string? search, string? selectProps, int? limit)
    {
        var httpClient = httpClientFactory.CreateClient("dummyjson_client");
        var fullUrl = CreateFullUrlWithQueryString("/products", search, selectProps, limit);
        var response = await httpClient.GetAsync(fullUrl);
        response.EnsureSuccessStatusCode();
        string json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ProductsResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    private string CreateFullUrlWithQueryString(string fullUrl, string? search, string? selectProps, int? limit)
    {
        if (search is not null)
        {
            fullUrl = $"{fullUrl}/search?q={search}";
        }

        return QueryHelpers.AddQueryString(fullUrl, new Dictionary<string, string?>()
        {
            { "select", selectProps?.ToString() },
            { "limit", limit?.ToString() }
        });
    }
}

