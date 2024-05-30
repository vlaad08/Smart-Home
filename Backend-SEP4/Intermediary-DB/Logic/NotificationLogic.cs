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

    public async Task<List<Notification>>? GetNotifications(string houseId)
    {
        try
        {
            return await repository.GetNotifications(houseId);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task SetBurglarNotification(string deviceId)
    {
        try
        {
            System.Console.WriteLine("BURGLAR IN HOUSE");
            await repository.AddBurglarNotification(deviceId,"BURGLAR IN HOUSE");
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}