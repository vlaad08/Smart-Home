using ConsoleApp1;
using DBComm.Logic.Interfaces;
using DBComm.Repository;
using DBComm.Shared;

namespace DBComm.Logic;

public class HumidityLogic : IHumidityLogic
{
    private ICommunicator communicator;

    private HumidityRepository repository;
    //maybe private
    public HumidityLogic()
    {
        communicator = Communicator.Instance;
        repository = new HumidityRepository();
    }
    
    public async Task<Humidity> getHumidity()
    {
        return await repository.getLates();
    }

    public void saveHumidity(Humidity humidity)
    {
        repository.update(humidity);
        
    }
}