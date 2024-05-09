using DBComm.Logic;
using DBComm.Shared;
using Microsoft.EntityFrameworkCore;

namespace DBComm.Repository;

public class HumidityRepository : IBaseRepository
{
    public Context Context;
    public HumidityRepository(Context context)
    {
        Context = context;
    }
    
    public async Task<Object> getOne(Object element)
    {
        try
        {
            IQueryable<HumidityReading> humidityReading = Context.HumidityReadings
                .Where(hr => hr.Room.DeviceId == element.ToString())  
                .OrderByDescending(hr => hr.ReadAt);

            HumidityReading? result = await humidityReading.FirstOrDefaultAsync();
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