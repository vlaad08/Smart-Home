using DBComm.Shared;

namespace DBComm.Repository;

public class TemperatureRepository : IBaseRepository
{
    public Task getOne<T>(T element)
    {
        
        throw new NotImplementedException();
    }

    public async Task<Temperature> getLates()
    {
        Temperature temperature = new Temperature
        {
            Value = 21,
            ReadAt = DateTime.Now
        };
        return temperature;
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