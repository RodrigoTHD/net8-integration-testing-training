using Microsoft.AspNetCore.Mvc;
using MyApp.ServiceMocking.Model;
using MyApp.ServiceMocking.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyApp.MockingDependencies.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService notificationService;

        public NotificationController(INotificationService notificationService)
        {
            this.notificationService = notificationService;
        }

        [HttpPost("Notify")]
        public async Task<IActionResult> Notify([FromBody] Notification notification)
        {
            if (notification == null || notification.IsEmpty)
            {
                return BadRequest("Notification is empty!");
            }


            var result = await notificationService.NotifyAsync(notification);
            return Ok(result);
        }
    }
}
