using AuthAPI.Models;

namespace AuthAPI.Services;

public interface IAuthenticationService
{
    Task<UserInfo?> LoginUser(LoginInfo loginInfo);
}
