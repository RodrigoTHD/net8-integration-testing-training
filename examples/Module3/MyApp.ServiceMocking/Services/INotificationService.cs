using MyApp.ServiceMocking.Model;

namespace MyApp.ServiceMocking.Services;

public interface INotificationService
{
    Task<bool> NotifyAsync(Notification notification);
}
