using AuthAPI.IntegrationTests.Contexts;
using AuthAPI.IntegrationTests.Utils;
using AuthAPI.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text;
using System.Text.Json;

namespace AuthAPI.IntegrationTests.Steps;

[Binding]
public class UserLoginAPISteps
{
    private readonly HttpClient client;
    private UserLoginContext userLoginContext;

    public UserLoginAPISteps(UserLoginContext UserLoginContext)
    {
        var application = new WebApplicationFactory<Program>();
        client = application.CreateClient();
        userLoginContext = UserLoginContext;
    }

    [Given("a user with username \"(.*)\" and password \"(.*)\"")]
    public void GivenAUserWithUsernameAndPassword(string username, string password)
    {
        userLoginContext.LoginInfo.ExpiresInMins = 10;

        if (LoginUserControllerUtils.ValidUsersMap.TryGetValue(username, out string? mapUser))
        {
            userLoginContext.LoginInfo.Username = mapUser;
        }

        if (LoginUserControllerUtils.ValidUsersMap.TryGetValue(password, out string? mapPass))
        {
            userLoginContext.LoginInfo.Password = mapPass;
        }
    }

    [When("the user requests the \"(.*)\" endpoint")]
    public async Task WhenTheUserRequestsTheEndpoint(string endpoint)
    {
        // Create the requestBody
        var jsonContent = new StringContent(
            JsonSerializer.Serialize(userLoginContext.LoginInfo),
            Encoding.UTF8,
            "application/json");

        userLoginContext.response = await client.PostAsync("/api/v1/LoginUser", jsonContent);
    }

    [Then("the response should be 200 OK")]
    public void ThenTheResponseShouldBeOk()
    {
        (userLoginContext.response!.StatusCode).Should().Be(HttpStatusCode.OK);
    }

    [Then("the response should be 401 Unauthorized")]
    public void ThenTheResponseShouldBeUnauthorized()
    {
        (userLoginContext.response!.StatusCode).Should().Be(HttpStatusCode.Unauthorized);
    }

    [Then("the response should contain a token")]
    public async Task ThenTheResponseShouldContainAToken()
    {
        var content = await userLoginContext.response!.Content.ReadAsStringAsync();
        UserInfo? userInfo = JsonSerializer.Deserialize<UserInfo>(
                content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        userInfo.Should().BeOfType<UserInfo>();
        userInfo.AccessToken.Should().NotBeEmpty();
    }

    [Then("the response message should be \"(.*)\"")]
    public async Task ThenTheResponseMessageShouldBe(string expectedMessage)
    {
        var content = await userLoginContext.response!.Content.ReadAsStringAsync();
        content.Should().Contain(expectedMessage);
    }
}
