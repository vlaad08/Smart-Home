using DBComm.Logic;
using DBComm.Shared;
using Microsoft.EntityFrameworkCore;

namespace DBComm.Repository;

public class LightRepository : ILigthRepository
{
    private Context _context;
    public LightRepository(Context context)
    {
        _context = context;
    }
    public async Task<LightReading> GetLatestLight(string deviceId)
    {
        try
        {
            IQueryable<LightReading> lightReadings = _context.light_reading
                .Where(lr => lr.Room.DeviceId == deviceId)  
                .OrderByDescending(lr => lr.ReadAt);

            LightReading? result = await lightReadings.FirstOrDefaultAsync();
            return result;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<ICollection<LightReading>> GetHistory( string deviceId, DateTime dateFrom, DateTime dateTo)
    {
        try
        {
            var query = _context.light_reading
                .Where(lr => lr.ReadAt >= dateFrom && lr.ReadAt <= dateTo && lr.Room.DeviceId == deviceId)
                .GroupBy(lr => lr.ReadAt.Date) // Group by date
                .Select(group => new LightReading()
                {
                   ReadAt = group.Key,
                   Value = group.Average(lr => lr.Value) // Calculate average light level for each group
                })
                .OrderBy(result => result.ReadAt); // Optional: Order by date

            var averageLightLevels = await query.ToListAsync();
            return averageLightLevels;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
    
    public async Task SaveLightReading(string deviceId,double value, DateTime readAt)
    {
        try
        {
            var room = await _context.room.FirstOrDefaultAsync(r => r.DeviceId == deviceId);
            if (room == null)
            {
                throw new Exception($"No such room with device {deviceId}");
            }
            LightReading lightReading = new LightReading(value, readAt)
            {
                Room = room
            };
        
            await _context.light_reading.AddAsync(lightReading);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception(e.Message);
        }
    }


}