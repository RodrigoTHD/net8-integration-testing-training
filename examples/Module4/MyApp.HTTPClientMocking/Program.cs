using Microsoft.Extensions.Options;
using MyApp.HTTPClientMocking.Options;
using MyApp.HTTPClientMocking.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<ApiServiceOptions>(builder.Configuration.GetSection(typeof(ApiServiceOptions).Name));
builder.Services.AddScoped<IApiServiceWithTypedClient, ApiServiceWithTypedClient>();
builder.Services.AddScoped<IApiServiceWithNamedClient, ApiServiceWithNamedClient>();

// Configuring a Typed HttpClient in the DI Container
builder.Services.AddHttpClient<IApiServiceWithTypedClient, ApiServiceWithTypedClient>((serviceProvider, client) =>
{
    var fhirClientOptions = serviceProvider.GetRequiredService<IOptions<ApiServiceOptions>>().Value;
    client.BaseAddress = new Uri(fhirClientOptions.BaseUrl);
});

// Configuring a Named HttpClient in the DI Container
builder.Services.AddHttpClient("ApiService_ClientName", (serviceProvider, client) =>
{
    var fhirClientOptions = serviceProvider.GetRequiredService<IOptions<ApiServiceOptions>>().Value;
    client.BaseAddress = new Uri(fhirClientOptions.BaseUrl);
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

// Make the Program class public using a partial class
// https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-9.0#basic-tests-with-the-default-webapplicationfactory
public partial class Program { }
