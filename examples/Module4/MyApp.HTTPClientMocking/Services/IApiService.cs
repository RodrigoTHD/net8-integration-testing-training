namespace MyApp.HTTPClientMocking.Services;

public interface IApiService
{
    Task<T?> GetDataAsync<T>(string route) where T : new();
}
