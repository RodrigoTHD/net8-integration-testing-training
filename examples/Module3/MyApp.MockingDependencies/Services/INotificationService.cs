using MyApp.MockingDependencies.Model;

namespace MyApp.MockingDependencies.Services;

public interface INotificationService
{
    Task<bool> NotifyAsync(Notification notification);
}
