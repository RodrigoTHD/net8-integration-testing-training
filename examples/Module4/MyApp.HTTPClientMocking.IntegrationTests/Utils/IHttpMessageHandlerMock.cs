namespace MyApp.HTTPClientMocking.IntegrationTests.Utils;

public interface IHttpMessageHandlerMock
{
    public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken);
}

