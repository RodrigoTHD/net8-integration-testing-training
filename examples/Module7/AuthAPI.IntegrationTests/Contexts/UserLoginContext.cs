using AuthAPI.Models;

namespace AuthAPI.IntegrationTests.Contexts;

public class UserLoginContext
{
    public LoginInfo LoginInfo { get; set; } = new();

    public HttpResponseMessage? response { get; set; }
}
