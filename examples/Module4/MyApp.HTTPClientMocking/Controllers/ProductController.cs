using Microsoft.AspNetCore.Mvc;
using MyApp.HTTPClientMocking.Services;

namespace MyApp.HTTPClientMocking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet("product/{id}")]
        public async Task<IActionResult> GetProduct([FromRoute] string id)
        {
            var result = await productService.GetProduct(id);
            return Ok(result);
        }

        [HttpGet("products")]
        public async Task<IActionResult> GetProducts([FromQuery] string? search, [FromQuery] string? selectProps, [FromQuery] int? limit)
        {
            var result = await productService.GetProducts(search, selectProps, limit);
            return Ok(result);
        }
    }
}
