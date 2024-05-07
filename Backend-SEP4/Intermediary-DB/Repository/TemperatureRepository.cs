using DBComm.Logic;
using DBComm.Shared;
using Microsoft.EntityFrameworkCore;

namespace DBComm.Repository;

public class TemperatureRepository : IBaseRepository
{
    public Context Context;
    public TemperatureRepository(Context context)
    {
        Context = context;
    }
    public Task getOne<T>(T element)
    {
        
        throw new NotImplementedException();
    }

    public async Task<TemperatureReading> getLatest(string hardwareId)
    {
        try
        {
            IQueryable<TemperatureReading> temperatureReadings = Context.TemperatureReadings
                .Where(tr => tr.Room.DeviceId == hardwareId)  
                .OrderByDescending(tr => tr.ReadAt);

            TemperatureReading result = await temperatureReadings.FirstOrDefaultAsync();
            return result;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        
    }

    public Task get<T>(T element)
    {
        throw new NotImplementedException();
    }

    public Task create<T>(T element)
    {
        throw new NotImplementedException();
    }

    public Task update<T>(T element)
    {
        throw new NotImplementedException();
    }

    public Task delete<T>(T element)
    {
        throw new NotImplementedException();
    }
}