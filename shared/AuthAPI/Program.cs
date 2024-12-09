using AuthAPI.Options;
using AuthAPI.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<ApiServiceOptions>(builder.Configuration.GetSection(typeof(ApiServiceOptions).Name));
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuring a Named HttpClient in the DI Container
builder.Services.AddHttpClient("dummyjson_client", (serviceProvider, client) =>
{
    var apiServiceOptions = serviceProvider.GetRequiredService<IOptions<ApiServiceOptions>>().Value;

    ArgumentException.ThrowIfNullOrWhiteSpace(apiServiceOptions.BaseUrl);

    client.BaseAddress = new Uri(apiServiceOptions.BaseUrl);
});

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
