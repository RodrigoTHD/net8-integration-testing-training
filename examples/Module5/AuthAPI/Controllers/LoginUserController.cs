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
            logger.LogInformation("User authentication request initiated.");

            try
            {
                var userInfo = await authenticationService.LoginUser(loginInfo);

                if (userInfo == null)
                {
                    return UnauthorizedUser(loginInfo);
                }

                logger.LogInformation($"Authentication successful for user ID '{userInfo.Id}'.");

                return Ok(userInfo);
            }
            catch (ArgumentNullException)
            {
                return UnauthorizedUser(loginInfo);
            }
            catch (Exception ex)
            {
                string message = $"Authentication failed for user '{loginInfo.Username}'. {ex.Message}";

                logger.LogError(message);

                return StatusCode(500, message);
            }
        }

        private IActionResult UnauthorizedUser(LoginInfo loginInfo)
        {
            string message = $"Authentication failed for user '{loginInfo.Username}'. Invalid username or password.";

            logger.LogWarning(message);

            return Unauthorized(message);
        }
    }
}
