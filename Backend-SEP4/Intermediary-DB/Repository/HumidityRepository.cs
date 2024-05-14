using DBComm.Logic;
using DBComm.Shared;
using Microsoft.EntityFrameworkCore;

namespace DBComm.Repository;

public class HumidityRepository : IHumidityRepository
{
    public Context Context;
    public HumidityRepository(Context context)
    {
        Context = context;
    }

    public async Task<HumidityReading> GetOne(string deviceId)
    {
        try
        {
            IQueryable<HumidityReading> humidityReading = Context.humidity_reading
                .Where(hr => hr.Room.DeviceId == deviceId)  
                .OrderByDescending(hr => hr.ReadAt);

            HumidityReading? result = await humidityReading.FirstOrDefaultAsync();
            return result;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<ICollection<HumidityReading>> GetHistory(string deviceId, DateTime dateFrom, DateTime dateTo)
    {
        try
        {
            var query = Context.humidity_reading
                .Where(lr => lr.ReadAt >= dateFrom && lr.ReadAt <= dateTo && lr.Room.DeviceId == deviceId)
                .GroupBy(lr => lr.ReadAt.Date) // Group by date
                .Select(group => new HumidityReading()
                {
                    ReadAt = group.Key,
                    Value = group.Average(lr => lr.Value) // Calculate average light level for each group
                })
                .OrderBy(result => result.ReadAt); // Optional: Order by date

            var averageHumidityLevels = await query.ToListAsync();
            return averageHumidityLevels;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}