using MyApp.HTTPClientMocking.Models;

namespace MyApp.HTTPClientMocking.Services;

public interface IProductService
{
    Task<Product?> GetProduct(string id);

    Task<ProductsResponse?> GetProducts(string? search, string? selectProps, int? limit);
}
