using Microsoft.AspNetCore.Mvc;
using MyApp.MockingDependencies.Model;
using MyApp.MockingDependencies.Services;

namespace MyApp.MockingDependencies.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiServiceWithClientFactoryController : ControllerBase
    {
        private readonly IApiServiceWithClientFactory apiServiceWithClientFactory;

        public ApiServiceWithClientFactoryController(IApiServiceWithClientFactory apiServiceWithClientFactory)
        {
            this.apiServiceWithClientFactory = apiServiceWithClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> GetTestData()
        {
            var result = await apiServiceWithClientFactory.GetDataAsync<ApiServiceResponse>("test");
            return Ok(result);
        }
    }
}
