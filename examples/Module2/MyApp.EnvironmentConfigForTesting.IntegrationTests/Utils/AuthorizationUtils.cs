using System.Net.Http.Headers;

namespace MyApp.EnvironmentConfigForTesting.IntegrationTests.Utils
{
    internal class AuthorizationUtils
    {
        public static AuthenticationHeaderValue CreateBasicAuth(string? id, string? secret)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes($"{id}:{secret}");
            string token = Convert.ToBase64String(plainTextBytes);
            return new AuthenticationHeaderValue("Basic", token);
        }
    }
}
