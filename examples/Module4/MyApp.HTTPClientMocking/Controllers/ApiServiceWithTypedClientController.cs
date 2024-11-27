using Microsoft.AspNetCore.Mvc;
using MyApp.HTTPClientMocking.Models;
using MyApp.HTTPClientMocking.Services;

namespace MyApp.HTTPClientMocking.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApiServiceWithTypedClientController : ControllerBase
    {
        private readonly IApiServiceWithTypedClient apiServiceWithTypedClient;

        public ApiServiceWithTypedClientController(IApiServiceWithTypedClient apiServiceWithTypedClient)
        {
            this.apiServiceWithTypedClient = apiServiceWithTypedClient;
        }

        [HttpGet]
        public async Task<IActionResult> GetTestData()
        {
            var result = await apiServiceWithTypedClient.GetDataAsync<ApiServiceResponse>("test");
            return Ok(result);
        }
    }
}
