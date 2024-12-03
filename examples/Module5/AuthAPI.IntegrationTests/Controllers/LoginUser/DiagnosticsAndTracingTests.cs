using AuthAPI.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Text.Json;

namespace AuthAPI.IntegrationTests.Controllers.LoginUser;

public class DiagnosticsAndTracingTests
{
    /// <summary>
    /// Custom Activity for HTTP Operations
    /// </summary>
    /// <returns></returns>
    [Test]
    public async Task shouldTraceRequestWithDiagnostics_LogsTraceInformation()
    {
        // Arrange
        var activity = new Activity("TestActivity");
        activity.Start();

        var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();

        // Create the requestBody
        var requestBody = new LoginInfo
        {
            Username = "user",
            Password = "pass",
            ExpiresInMins = 10
        };
        var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        activity.AddTag("requestBody", requestBody);

        // Act
        var response = await client.PostAsync("/api/v1/LoginUser", jsonContent);
        activity.AddTag("response.status", response.StatusCode);
        activity.AddTag("RequestMessage", response.RequestMessage);
        activity.Stop();


        // Assert
        // Verify the error message
        var stringResponse = await response.Content.ReadAsStringAsync();
        StringAssert.Contains("Invalid username or password", stringResponse);

        // Verify that the server responds with 401 Unauthorized to indicate that the credentials provided do not grant access.
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));

        // Trace information for debugging
        Console.WriteLine($"\n");
        Console.WriteLine($"Activity ID:            {activity.Id}");
        Console.WriteLine($"Activity.DisplayName:   {activity.DisplayName}");
        Console.WriteLine($"Activity.Kind:          {activity.Kind}");
        Console.WriteLine($"Activity.StartTime:     {activity.StartTimeUtc}");
        Console.WriteLine($"Activity.Duration:      {activity.Duration}");
        Console.WriteLine($"Activity.Tags:          {SerializeKeyValuePair(activity.TagObjects)}");
    }

    private string SerializeKeyValuePair(IEnumerable<KeyValuePair<string, object?>> tagObjects) =>
        JsonSerializer.Serialize(
            tagObjects.ToDictionary(pair => pair.Key, pair => pair.Value),
            new JsonSerializerOptions { WriteIndented = true });
}
