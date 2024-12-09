using Microsoft.Extensions.Options;
using MyApp.HTTPClientMocking.Options;
using MyApp.HTTPClientMocking.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<ApiServiceOptions>(builder.Configuration.GetSection(typeof(ApiServiceOptions).Name));
builder.Services.AddScoped<IApiServiceWithTypedClient, ApiServiceWithTypedClient>();
builder.Services.AddScoped<IApiServiceWithNamedClient, ApiServiceWithNamedClient>();
builder.Services.AddScoped<IProductService, ProductService>();

// Configuring a Typed HttpClient in the DI Container
builder.Services.AddHttpClient<IApiServiceWithTypedClient, ApiServiceWithTypedClient>((serviceProvider, client) =>
{
    var fhirClientOptions = serviceProvider.GetRequiredService<IOptions<ApiServiceOptions>>().Value;
    client.BaseAddress = new Uri(fhirClientOptions.BaseUrl);
});

// Configuring a Named HttpClient in the DI Container
builder.Services.AddHttpClient("dummyjson_client", (serviceProvider, client) =>
{
    var fhirClientOptions = serviceProvider.GetRequiredService<IOptions<ApiServiceOptions>>().Value;
    client.BaseAddress = new Uri(fhirClientOptions.BaseUrl);
    client.DefaultRequestHeaders.Add("X-Custom-Header", "CustomValue");
    client.DefaultRequestHeaders.Add("Authorization", "Bearer some_access_token");
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

// Expose the implicitly defined Program class to the test project.
// https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-9.0#basic-tests-with-the-default-webapplicationfactory
public partial class Program { }
