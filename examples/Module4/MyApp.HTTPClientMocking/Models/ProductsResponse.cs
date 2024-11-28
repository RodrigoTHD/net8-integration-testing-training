namespace MyApp.HTTPClientMocking.Models;

public class ProductsResponse
{
    public IEnumerable<Product> Products { get; set; } = Enumerable.Empty<Product>();

    public int Total { get; set; } = 0;
}
