using DBComm.Shared;

namespace DBComm.Repository;

public interface INotificationRepository
{
    Task<List<Notification>>? GetNotifications();
}