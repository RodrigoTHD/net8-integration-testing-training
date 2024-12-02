using AuthAPI.Models;
using AuthAPI.Services;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

using System.Net;
using System.Text;
using System.Text.Json;

namespace AuthAPI.IntegrationTests.Controllers.LoginUserController;

public class ErrorHandlingTests
{
    /// <summary>
    /// Using FakeItEasy with NUnit, simulate a dependency failure and verify that AuthAPI
    /// correctly returns an HTTP 500 status code.
    /// </summary>
    /// <returns></returns>
    [Test]
    public async Task ShouldReturnInternalServerErrorWhenLoginUserFails()
    {
        // Arrange
        // The FakeItEasy library is used to simulate a failing service
        var fakeAuthenticationService = A.Fake<IAuthenticationService>();
        A.CallTo(() => fakeAuthenticationService.LoginUser(A<LoginInfo>.Ignored)).Throws<Exception>();

        var factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Add fake service
                    services.AddScoped(_ => fakeAuthenticationService);
                });
            });

        var client = factory.CreateClient();

        // Create the requestBody
        var requestBody = new LoginInfo
        {
            Username = "user",
            Password = "pass",
            ExpiresInMins = 10
        };
        var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync("/api/v1/LoginUser", jsonContent);

        // Assert
        // Verify that the API properly handles the exception and returns a 500 status code
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
    }

    /// <summary>
    /// Authentication failure, such as incorrect credentials (username or password).
    ///
    /// </summary>
    /// <returns></returns>
    [Test]
    public async Task ShouldReturnUnauthorizedWhenLoginUserFails()
    {
        // Arrange
        var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();

        // Create the requestBody
        var requestBody = new LoginInfo
        {
            Username = "",
            Password = "",
            ExpiresInMins = 10
        };
        var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync("/api/v1/LoginUser", jsonContent);

        // Assert
        // Verify that the server responds with 401 Unauthorized to indicate that the credentials provided do not grant access.
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }
}
