using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;

namespace MyApp.EnvironmentConfigForTesting.Services;

public class FhirService : IFhirService
{
    private readonly FhirClient fhirClient;

    public FhirService(FhirClient fhirClient)
    {
        this.fhirClient = fhirClient;
    }

    public Task<TResource?> CreateAsync<TResource>(TResource resource)
          where TResource : Resource =>
          fhirClient.CreateAsync(resource);

    public Task<Resource?> GetAsync(string url)
        => fhirClient.GetAsync(url);

    public Task<Bundle?> SearchUsingPostAsync(SearchParams searchParams, string? resourceType = null)
        => fhirClient.SearchUsingPostAsync(searchParams, resourceType);

    public Task<Bundle?> TransactionAsync(Bundle bundle) =>
        fhirClient.TransactionAsync(bundle);

    public Task<TResource?> UpdateAsync<TResource>(TResource resource, bool versionAware = false)
       where TResource : Resource =>
        fhirClient.UpdateAsync(resource, versionAware);
}

