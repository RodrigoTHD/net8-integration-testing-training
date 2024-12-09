using Hl7.Fhir.Rest;
using Microsoft.Extensions.Options;
using MyApp.EnvironmentConfigForTesting.Options;
using MyApp.EnvironmentConfigForTesting.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configure FhirClientOptions in the container.
builder.Services.Configure<FhirClientOptions>(builder.Configuration.GetSection(typeof(FhirClientOptions).Name));

builder.Services.AddScoped<IFhirService, FhirService>((serviceProvider) =>
{
    var fhirClientOptions = serviceProvider.GetRequiredService<IOptions<FhirClientOptions>>().Value;
    return new FhirService(new FhirClient(fhirClientOptions.BaseUrl));
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
