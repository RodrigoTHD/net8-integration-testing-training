
namespace MyApp.MockingDependencies.Services;

public class EmailService : IEmailService
{
    public Task<bool> SendEmailAsync(string to, string subject, string body)
    {
        if (IsNotEmptyEmail(to, subject, body))
        {
            // TODO: sendo email.
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }

    private bool IsNotEmptyEmail(string to, string subject, string body)
        => !string.IsNullOrWhiteSpace(to) && !string.IsNullOrWhiteSpace(subject) && !string.IsNullOrWhiteSpace(body);
}
