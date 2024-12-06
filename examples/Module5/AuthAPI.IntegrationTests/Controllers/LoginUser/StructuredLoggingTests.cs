using AuthAPI.Controllers;
using AuthAPI.Models;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using NUnit.Framework.Internal;
using System.Text;
using System.Text.Json;

namespace AuthAPI.IntegrationTests.Controllers.LoginUser;

public class StructuredLoggingTests
{
    /// <summary>
    /// Capture and verify logs during an integration test using a fake logger.
    /// </summary>
    /// <returns></returns>
    [Test]
    public async Task ShouldLoginUserReturnsLogsErrorWhenValidationFails()
    {
        // Arrange
        // Create a mock ILogger for the controller LoginUserController.
        var fakeLogger = A.Fake<ILogger<LoginUserController>>();

        // Stores the logs to be checked later.
        var logs = new List<KeyValuePair<LogLevel, string>>();
        A.CallTo(fakeLogger)
         .Where(call => call.Method.Name == "Log")
         .Invokes(obj =>
         {
             // Get logLevel
             LogLevel? logLevel = obj.Arguments.Get<LogLevel>("logLevel");
             // Get log message
             var state = obj.Arguments.Get<IReadOnlyList<KeyValuePair<string, object?>>>("state");
             string? message = state?.ToString();
             if (logLevel is LogLevel && message is not null)
             {
                 // Store the called logs.
                 logs.Add(new KeyValuePair<LogLevel, string>((LogLevel)logLevel, message));
             }
         });

        var factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Replace ILogger with the fake logger
                    services.AddScoped(_ => fakeLogger);
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
        await client.PostAsync("/api/v1/LoginUser", jsonContent);

        // Verify that the log was called by logLevel and message.
        Assert.That(logs.Any(log =>
            log.Key == LogLevel.Information &&
            log.Value.Contains($"User authentication request initiated for user '{requestBody.Username}'.")), Is.True);

        Assert.That(logs.Any(log =>
            log.Key == LogLevel.Warning &&
            log.Value.Contains($"Authentication failed for user '{requestBody.Username}'. Invalid credentials.")), Is.True);
    }
}
