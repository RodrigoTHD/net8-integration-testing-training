using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;

namespace MyApp.EnvironmentConfigForTesting.Services;

public interface IFhirService
{
    Task<TResource?> CreateAsync<TResource>(TResource resource)
        where TResource : Resource;

    Task<Resource?> GetAsync(string url);

    Task<Bundle?> SearchUsingPostAsync(SearchParams searchParams, string? resourceType = null);

    Task<Bundle?> TransactionAsync(Bundle bundle);

    Task<TResource?> UpdateAsync<TResource>(TResource resource, bool versionAware = false)
        where TResource : Resource;
}
