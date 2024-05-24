using DBComm.Logic;
using DBComm.Shared;
using Microsoft.EntityFrameworkCore;

namespace DBComm.Repository;

public class TemperatureRepository : ITemperatureRepository
{
    private Context _context;
    public TemperatureRepository(Context context)
    {
        _context = context;
    }

    public async Task<TemperatureReading> GetOne(string deviceId)
    {
        try
        {
            IQueryable<TemperatureReading> temperatureReadings = _context.temperature_reading
                .Where(tr => tr.Room.DeviceId == deviceId)  
                .OrderByDescending(tr => tr.ReadAt);

            TemperatureReading? result = await temperatureReadings.FirstOrDefaultAsync();
            return result;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<ICollection<TemperatureReading>> GetHistory(string deviceId, DateTime dateFrom, DateTime dateTo)
    {
        try
        {
            var query = _context.temperature_reading
                .Where(lr => lr.ReadAt >= dateFrom && lr.ReadAt <= dateTo && lr.Room.DeviceId == deviceId)
                .GroupBy(lr => lr.ReadAt.Date) // Group by date
                .Select(group => new TemperatureReading()
                {
                    ReadAt = group.Key,
                    Value = group.Average(lr => lr.Value) // Calculate average light level for each group
                })
                .OrderBy(result => result.ReadAt); // Optional: Order by date

            var averageTemperatureLevels = await query.ToListAsync();
            return averageTemperatureLevels;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    

    public async Task SaveTemperatureReading(string deviceId,double value, DateTime readAt)
    {
        try
        {
            var room = await _context.room.FirstOrDefaultAsync(r => r.DeviceId == deviceId);
            if (room == null)
            {
                throw new Exception($"No such room with device {deviceId}");
            }
            TemperatureReading temperatureReading = new TemperatureReading(value, readAt)
            {
                Room = room
            };
        
            await _context.temperature_reading.AddAsync(temperatureReading);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception(e.Message);
        }
    }

}