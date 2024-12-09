using FakeItEasy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MyApp.ServiceMocking.Model;
using MyApp.ServiceMocking.Services;
using System.Text;
using System.Text.Json;

namespace MyApp.ServiceMocking.IntegrationTests.Controllers;


public class NotificationControllerTests
{
    /// <summary>
    /// Mock an IEmailService dependency that sends emails
    /// </summary>
    /// <returns></returns>
    [Test]
    public async Task ShouldReturnMockedDataFromDIService()
    {
        // Arrange
        // Create a fake instance of IEmailService
        var fakeEmailService = A.Fake<IEmailService>();

        // Set up the fake behavior
        A.CallTo(() => fakeEmailService.SendEmailAsync(A<string>.Ignored, A<string>.Ignored, A<string>.Ignored)).Returns(true);

        // Override the DI service with the mocked instance
        var factoryWithMock = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Replace IEmailService with the mocked implementation
                services.AddScoped(_ => fakeEmailService);
            });
        });

        // Create a test client
        var client = factoryWithMock.CreateClient();

        // Create the requestBody
        var requestBody = new Notification
        {
            To = "user@example.com",
            Subject = "Notification",
            Body = "Test message"
        };
        var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

        // Act
        // Send a POST request
        var response = await client.PostAsync("/Notification/Notify", jsonContent);

        // Assert
        // Verify the response status code
        Assert.That(response.IsSuccessStatusCode, Is.True);

        // Verify that the mocked service was called
        A.CallTo(() => fakeEmailService.SendEmailAsync(requestBody.To, requestBody.Subject, requestBody.Body))
            .MustHaveHappenedOnceExactly();
    }
}
