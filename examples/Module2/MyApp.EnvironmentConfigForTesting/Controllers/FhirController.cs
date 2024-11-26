using Microsoft.AspNetCore.Mvc;
using MyApp.EnvironmentConfigForTesting.Services;

namespace MyApp.EnvironmentConfigForTesting.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FhirController : ControllerBase
    {
        private readonly IFhirService fhirService;

        public FhirController(IFhirService fhirService)
        {
            this.fhirService = fhirService;
        }

        [HttpGet("GetPatients")]
        public async Task<IActionResult> GetPatients([FromQuery] int count = 10)
        {
            var result = await fhirService.GetAsync($"Patient?_count={count}");
            return Ok(result);
        }
    }
}
