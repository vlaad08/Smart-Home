using DBComm.Logic.Interfaces;
using DBComm.Repository;
using DBComm.Shared;

namespace DBComm.Logic;

public class NotificationLogic : INotificationLogic
{
    private INotificationRepository repository;

    public NotificationLogic(INotificationRepository repository)
    {
        this.repository = repository;
    }

    public async Task<List<Notification>>? GetNotifications()
    {
        try
        {
            return await repository.GetNotifications();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw new Exception(e.Message);
        }
    }
}