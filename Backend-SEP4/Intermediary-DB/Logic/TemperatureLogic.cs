using ConsoleApp1;
using DBComm.Logic.Interfaces;
using DBComm.Repository;
using DBComm.Shared;

namespace DBComm.Logic;

public class TemperatureLogic : ITemperatureLogic
{
    private ICommunicator communicator;

    private ITemperatureRepository repository;
    public TemperatureLogic(ITemperatureRepository repository)
    {
        communicator = Communicator.Instance;
        this.repository = repository;
    }
    public async Task<TemperatureReading> getLatestTemperature(string hardwareId)
    {
        return await repository.GetOne(hardwareId);
    }

    public void saveTemperature(TemperatureReading temperatureReading)
    {
        //repository.update(temperatureReading);
        
    }

    public async Task<ICollection<TemperatureReading>> getTemperatureHistory(string hardwareId, DateTime dateFrom, DateTime dateTo)
    {
        return await repository.GetHistory(hardwareId, dateFrom, dateTo);
    }

    public async Task setTemperature(string hardwareId, int level)
    {
        communicator.setTemperature(hardwareId, level);
    }
}