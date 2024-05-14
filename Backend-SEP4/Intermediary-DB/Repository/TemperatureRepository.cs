using DBComm.Logic;
using DBComm.Shared;
using Microsoft.EntityFrameworkCore;

namespace DBComm.Repository;

public class TemperatureRepository : ITemperatureRepository
{
    public Context Context;
    public TemperatureRepository(Context context)
    {
        Context = context;
    }

    public async Task<TemperatureReading> GetOne(string deviceId)
    {
        try
        {
            IQueryable<TemperatureReading> temperatureReadings = Context.temperature_reading
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
            var query = Context.temperature_reading
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
}