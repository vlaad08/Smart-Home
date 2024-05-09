using DBComm.Logic;
using DBComm.Shared;
using Microsoft.EntityFrameworkCore;

namespace DBComm.Repository;

public class NotificationRepository : INotificationRepository
{
    private Context context;

    public NotificationRepository(Context context)
    {
        this.context = context;
    }
    public async Task<List<Notification>> GetNotifications()
    {
        try
        {
            // Convert DateTime.Today to UTC
            var todayUtc = DateTime.UtcNow.Date;
            var tomorrowUtc = todayUtc.AddDays(1);

            IQueryable<Notification> notificationReadings = context.Notifications
                .Where(nr => nr.SendAt >= todayUtc && nr.SendAt < tomorrowUtc);

            List<Notification> result = await notificationReadings.ToListAsync();
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw new Exception(e.Message);
        }
    }
}



