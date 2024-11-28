using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace MyApp.HTTPClientMocking.IntegrationTests.Controllers.ProductController;

public class StatusCodesTests
{
    private WebApplicationFactory<Program> factory;

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
    /// Testing Status Codes OK.
    ///
    /// Ensure the API returns the correct status codes based on the request.
    /// </summary>
    /// <returns></returns>
    [Test]
    public async Task ShouldGetProductReturn200WhenIdIsValid()
    {
        // Arrange
        var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync($"/api/Product/product/1");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    /// <summary>
    /// Testing Status Codes NoContent.
    ///
    /// Ensure the API returns the correct status codes based on the request.
    /// </summary>
    /// <returns></returns>
    [Test]
    public async Task ShouldGetProductReturn204WhenIdIsInvalid()
    {
        // Arrange
        var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync($"/api/Product/product/invalid_id");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }
}
