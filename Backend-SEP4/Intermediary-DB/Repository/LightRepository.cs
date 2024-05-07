using DBComm.Shared;

namespace DBComm.Repository;

public class LightRepository : IBaseRepository
{
    public async Task<Light> getLatest()
    {
        Light light = new Light
        {
          test = "Test"
        };
        return light;
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