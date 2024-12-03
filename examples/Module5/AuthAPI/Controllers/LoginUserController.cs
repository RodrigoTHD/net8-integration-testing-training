using AuthAPI.Models;
using AuthAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AuthAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LoginUserController : ControllerBase
    {
        private readonly ILogger<LoginUserController> logger;
        private readonly IAuthenticationService authenticationService;

        public LoginUserController(ILogger<LoginUserController> logger, IAuthenticationService authenticationService)
        {
            this.logger = logger;
            this.authenticationService = authenticationService;
        }

        [HttpPost]
        public async Task<IActionResult> LoginUser([FromBody][Required] LoginInfo loginInfo)
        {
            logger.LogInformation($"User authentication request initiated for user '{loginInfo.Username}'.");

            try
            {
                var (userInfo, httpResponseMessage) = await authenticationService.LoginUser(loginInfo);

                if (userInfo is UserInfo)
                {
                    logger.LogInformation($"Authentication successful for user ID '{userInfo.Id}'.");

                    return Ok(userInfo);
                }

                if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized || httpResponseMessage.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    return UnauthorizedUserResponse(loginInfo);
                }

                return Problem(
                    title: httpResponseMessage.ReasonPhrase,
                    statusCode: ((int)httpResponseMessage.StatusCode),
                    detail: CreateDetailError(loginInfo));
            }
            catch (ArgumentNullException)
            {
                return UnauthorizedUserResponse(loginInfo);
            }
            catch (Exception ex)
            {
                string message = $"{CreateDetailError(loginInfo)}. {ex.Message}";

                logger.LogError(message);

                return StatusCode(500, message);
            }
        }

        private string CreateDetailError(LoginInfo loginInfo)
            => $"Authentication failed for user '{loginInfo.Username}'";


        private IActionResult UnauthorizedUserResponse(LoginInfo loginInfo)
        {
            string message = $"{CreateDetailError(loginInfo)}. Invalid username or password.";

            logger.LogWarning(message);

            return Unauthorized(message);
        }
    }
}
