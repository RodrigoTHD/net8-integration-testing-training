using Microsoft.AspNetCore.Mvc;
using MyApp.HTTPClientMocking.Models;
using MyApp.HTTPClientMocking.Services;

namespace MyApp.HTTPClientMocking.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiServiceWithNamedClientController : ControllerBase
    {
        private readonly IApiServiceWithNamedClient apiServiceWithNamedClient;

        public ApiServiceWithNamedClientController(IApiServiceWithNamedClient apiServiceWithNamedClient)
        {
            this.apiServiceWithNamedClient = apiServiceWithNamedClient;
        }

        [HttpGet]
        public async Task<IActionResult> GetTestData()
        {
            var result = await apiServiceWithNamedClient.GetDataAsync<ApiServiceResponse>("test");
            return Ok(result);
        }
    }
}
