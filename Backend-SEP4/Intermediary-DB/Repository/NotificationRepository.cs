using DBComm.Logic;
using DBComm.Shared;
using Microsoft.EntityFrameworkCore;

namespace DBComm.Repository;

public class NotificationRepository : INotificationRepository
{
    private Context _context;

    public NotificationRepository(Context context)
    {
        this._context = context;
    }
    public async Task<List<Notification>> GetNotifications(string houseId)
    {
        try
        {
            var todayUtc = DateTime.UtcNow.Date.AddHours(2);
            var tomorrowUtc = todayUtc.AddDays(1);

            IQueryable<Notification> notificationReadings = _context.notification
                .Where(nr => nr.SendAt >= todayUtc && nr.SendAt < tomorrowUtc && nr.Home.Id.Equals(houseId));

            List<Notification> result = await notificationReadings.ToListAsync();
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw new Exception(e.Message);
        }
    }

    public async Task AddNotification(string houseId, string message)
    {
        try
        {
            Home? home = await _context.home.FindAsync(houseId);
            if (home == null)
            {
                throw new Exception("Home doesn't exist");
            }

            Notification notification = new Notification(home, message);
            _context.Add(notification);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task AddBurglarNotification(string deviceId, string message)
    {
        try
        {
            string houseId = _context.room.Include(r=>r.Home).FirstOrDefaultAsync(r => r.DeviceId == deviceId).Result.Home.Id;
            Home? home = await _context.home.FindAsync(houseId);
            if (home == null)
            {
                throw new Exception("Home doesn't exist");
            }

            Notification notification = new Notification(home, message);
            _context.Add(notification);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}



