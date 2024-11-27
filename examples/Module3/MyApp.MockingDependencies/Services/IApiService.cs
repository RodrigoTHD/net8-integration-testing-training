namespace MyApp.MockingDependencies.Services
{
    public interface IApiService
    {
        Task<T?> GetDataAsync<T>(string route) where T : new();
    }
}
