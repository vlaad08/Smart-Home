using ConsoleApp1;
using DBComm.Logic.Interfaces;
using DBComm.Repository;
using DBComm.Shared;

namespace DBComm.Logic;

public class TemperatureLogic : ITemperatureLogic
{
    private ICommunicator communicator;

    private IBaseRepository repository;
    public TemperatureLogic(IBaseRepository repository)
    {
        communicator = Communicator.Instance;
        this.repository = repository;
    }
    public async Task<TemperatureReading> getLatestTemperature(string hardwareId)
    {
        return (TemperatureReading)await repository.getOne(hardwareId);
    }

    public void saveTemperature(TemperatureReading temperatureReading)
    {
        repository.update(temperatureReading);
        
    }
}