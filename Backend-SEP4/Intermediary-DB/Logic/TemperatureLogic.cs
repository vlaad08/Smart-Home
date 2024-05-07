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
        repository = new TemperatureRepository(new Context());
    }
    public async Task<TemperatureReading> getLatestTemperature(string hardwareId)
    {
        return await repository.getLatest(hardwareId);
    }

    public void saveTemperature(TemperatureReading temperatureReading)
    {
        repository.update(temperatureReading);
        
    }
}