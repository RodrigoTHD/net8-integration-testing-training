using Microsoft.AspNetCore.Mvc.Testing;
using MyApp.HTTPClientMocking.Models;
using System.Text.Json;

namespace MyApp.HTTPClientMocking.IntegrationTests.Controllers.ProductController;

public class QueryParametersTests
{
    private WebApplicationFactory<Program>? factory;

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
    /// Testing Query Parameters
    /// 
    /// Ensure that the server processes and interprets query parameters correctly and returns the expected response.
    /// </summary>
    /// <returns></returns>
    [Test]
    public async Task ShouldGetProductsWithSearchParametersReturnsFilteredProducts()
    {
        // Arrange
        var client = factory!.CreateClient();
        string search = "Powder";

        // Act
        var response = await client.GetAsync($"/api/Product/products?search={search}");

        // Assert
        var jsonResponse = await response.Content.ReadAsStringAsync();

        // Deserialize jsonResponse into ProductsResponse
        var apiServiceResponse = JsonSerializer.Deserialize<ProductsResponse>(
            jsonResponse,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        // Verify that the mock returns the expected response
        Assert.That(apiServiceResponse, Is.Not.Null);
        Assert.That(apiServiceResponse?.Total, Is.EqualTo(2));
    }

    [Test]
    public async Task ShouldGetProductsWithLimitParameterReturnsProducts()
    {
        // Arrange
        var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync($"/api/Product/products?limit=3");

        // Assert
        var jsonResponse = await response.Content.ReadAsStringAsync();

        // Deserialize jsonResponse into ProductsResponse
        var apiServiceResponse = JsonSerializer.Deserialize<ProductsResponse>(
            jsonResponse,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        // Verify that the mock returns the expected response
        Assert.That(apiServiceResponse, Is.Not.Null);
        Assert.That(apiServiceResponse?.Products.Count(), Is.EqualTo(3));
    }
}
