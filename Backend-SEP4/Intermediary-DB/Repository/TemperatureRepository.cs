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

    public async Task<Object?> getOne(Object element)
    {
        try
        {
            IQueryable<TemperatureReading> temperatureReadings = Context.TemperatureReadings
                .Where(tr => tr.Room.DeviceId == element.ToString())  
                .OrderByDescending(tr => tr.ReadAt);

            TemperatureReading? result = await temperatureReadings.FirstOrDefaultAsync();
            return result;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public Task<Object?> get(Object element)
    {
        throw new NotImplementedException();
    }

    public Task<Object?> create(Object element)
    {
        throw new NotImplementedException();
    }

    public Task<Object?> update(Object element)
    {
        throw new NotImplementedException();
    }

    public Task<Object?> delete(Object element)
    {
        throw new NotImplementedException();
    }
}