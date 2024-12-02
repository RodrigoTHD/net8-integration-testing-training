using AuthAPI.Models;
using AuthAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> logger;
        private readonly IAuthenticationService authenticationService;

        public AuthenticationController(ILogger<AuthenticationController> logger, IAuthenticationService authenticationService)
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
                    string message = $"Authentication failed for user '{loginInfo.Username}'. Invalid username or password.";

                    logger.LogWarning(message);

                    return Unauthorized(message);
                }

                logger.LogInformation($"Authentication successful for user ID '{userInfo.Id}'.");

                return Ok(userInfo);
            }
            catch (Exception ex)
            {
                string message = $"Authentication failed for user '{loginInfo.Username}'. {ex.Message}";

                logger.LogError(message);

                return BadRequest(message);
            }
        }
    }
}
