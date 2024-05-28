using DBComm.Shared;

namespace DBComm.Repository;

public interface INotificationRepository
{
    Task<List<Notification>>? GetNotifications(string houseId);
    Task AddNotification(string houseId, string message);
    Task AddBurglarNotification(string deviceId, string message);
}