using FakeItEasy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MyApp.HTTPClientMocking.Services;
using System.Net;

namespace MyApp.HTTPClientMocking.IntegrationTests.Limitations;
public class HttpClientMockLimitationTest
{
    [Test]
    public async Task ShouldShowHttpClientMockLimitation()
    {
        // Arrange
        var fakeHandler = A.Fake<HttpMessageHandler>();

        // =============================================================================================
        // HttpClient mock limitation
        //
        // Error CS0122 'HttpMessageHandler.Send(HttpRequestMessage, CancellationToken)'
        // is inaccessible due to its protection level
        //
        // Workaround:
        //      Using MockHttpMessageHandler from RichardSzalay.MockHttp library.
        //
        //A.CallTo(() => fakeHandler.Send(A<HttpRequestMessage>.Ignored, A<CancellationToken>.Ignored))
        //.Returns(new HttpResponseMessage
        //{
        //    StatusCode = HttpStatusCode.OK,
        //    Content = new StringContent("Mocked Data")
        //});
        // =============================================================================================

        // Initialize the WebApplicationFactory
        var factory = new WebApplicationFactory<Program>();

        var factoryWithMock = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddHttpClient<IApiServiceWithTypedClient, ApiServiceWithTypedClient>()
                            .ConfigurePrimaryHttpMessageHandler(() => fakeHandler);
            });
        });

        var client = factoryWithMock.CreateClient();

        // Act
        var response = await client.GetAsync("/ApiServiceWithTypedClient");

        // Assert
        Assert.That(
            response.StatusCode,
            Is.EqualTo(HttpStatusCode.InternalServerError),
            "Because HttpMessageHandler does not have mock returns configured");
    }
}
