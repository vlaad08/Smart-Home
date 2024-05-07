using DBComm.Shared;

namespace DBComm.Repository;

public class HumidityRepository : IBaseRepository
{
    
    public async Task<Humidity> getLates()
    {
        Humidity humidity = new Humidity()
        {
            Value = 65,
            ReadAt = DateTime.Now
        };
        return humidity;
    }
    
    public Task getOne<T>(T element)
    {
        throw new NotImplementedException();
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