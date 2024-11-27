using MyApp.MockingDependencies.Model;

namespace MyApp.MockingDependencies.Services;

public class NotificationService : INotificationService
{
    private readonly IEmailService emailService;

    public NotificationService(IEmailService emailService)
    {
        this.emailService = emailService;
    }

    public async Task<bool> NotifyAsync(Notification notification)
    {
        return await emailService.SendEmailAsync(notification.To, notification.Subject, notification.Body);
    }
}
