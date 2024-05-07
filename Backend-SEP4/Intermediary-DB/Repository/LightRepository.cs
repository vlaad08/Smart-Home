using DBComm.Logic;
using DBComm.Shared;
using Microsoft.EntityFrameworkCore;

namespace DBComm.Repository;

public class LightRepository : IBaseRepository
{
    public Context Context;
    public LightRepository(Context context)
    {
        Context = context;
    }

    public async Task<Object?> getOne(Object element)
    {
        try
        {
            IQueryable<HumidityReading> lightReadings = Context.HumidityReadings
                .Where(tr => tr.Room.DeviceId == element.ToString())  
                .OrderByDescending(tr => tr.ReadAt);

            HumidityReading? result = await lightReadings.FirstOrDefaultAsync();
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