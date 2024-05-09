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

    
    public void SetTemperatureLevel(string hardwareId, int temperatureLevel){
        if (temperatureLevel < 0 || temperatureLevel > 6)
        {
            throw new HttpRequestException("Invalid temperature level. Temperature level must be between 0 and 6.");
        }

        communicator.setTemperature(temperatureLevel.ToString());
    }
}