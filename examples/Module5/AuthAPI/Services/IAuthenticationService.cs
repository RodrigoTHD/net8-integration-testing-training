using AuthAPI.Models;

namespace AuthAPI.Services;

public interface IAuthenticationService
{
    Task<(UserInfo?, HttpResponseMessage)> LoginUser(LoginInfo loginInfo);
}
