using ConsoleApp1;
using DBComm.Logic.Interfaces;
using DBComm.Repository;
using DBComm.Shared;

namespace DBComm.Logic;

public class TemperatureLogic : ITemperatureLogic
{
    private ICommunicator communicator;

    private TemperatureRepository repository;
    //maybe private
    public TemperatureLogic()
    {
        communicator = Communicator.Instance;
        repository = new TemperatureRepository();
    }
    
    public async Task<Temperature> getTemperature()
    {
        return await repository.getLates();
    }

    public void saveTemperature(Temperature temperature)
    {
        repository.update(temperature);
        
    }
}