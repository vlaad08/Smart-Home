using DBComm.Logic;
using DBComm.Shared;
using Microsoft.EntityFrameworkCore;

namespace DBComm.Repository;

public class LightRepository : ILigthRepository
{
    private Context Context;
    public LightRepository(Context context)
    {
        Context = context;
    }
    public async Task<LightReading> GetOne(string deviceId)
    {
        try
        {
            IQueryable<LightReading> lightReadings = Context.light_reading
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
            var query = Context.light_reading
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

}