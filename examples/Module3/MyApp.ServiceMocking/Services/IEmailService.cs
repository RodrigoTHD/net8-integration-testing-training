namespace MyApp.ServiceMocking.Services;

public interface IEmailService
{
    Task<bool> SendEmailAsync(string to, string subject, string body);
}
