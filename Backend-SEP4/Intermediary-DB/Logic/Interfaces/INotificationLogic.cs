using DBComm.Shared;

namespace DBComm.Logic.Interfaces;

public interface INotificationLogic
{
    public Task<List<Notification>>? GetNotifications(string houseId);
}