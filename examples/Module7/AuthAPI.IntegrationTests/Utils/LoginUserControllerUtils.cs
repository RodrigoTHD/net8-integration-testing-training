namespace AuthAPI.IntegrationTests.Utils;

public static class LoginUserControllerUtils
{
    // Add custom initial values
    public static readonly IReadOnlyDictionary<string, string> ValidUsersMap = new Dictionary<string, string>
    {
        { "validUser", "charlottem" },
        { "validPass", "charlottempass" }
    };
}
