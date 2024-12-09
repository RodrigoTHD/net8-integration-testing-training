namespace MyApp.HTTPClientMocking.IntegrationTests.Utils;

/// <summary>
/// HttpMessageHandler mock class (Only For testing purpose).
/// </summary>
public class HttpMessageHandlerMock : HttpMessageHandler
{
    public IHttpMessageHandlerMock? Mock { get; set; }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) =>
        Mock!.SendAsync(request, cancellationToken);
}
