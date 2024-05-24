using ConsoleApp1;
using DBComm.Logic.Interfaces;
using DBComm.Repository;
using DBComm.Shared;

namespace DBComm.Logic;

public class TemperatureLogic : ITemperatureLogic
{
    private ICommunicator _communicator;

    private ITemperatureRepository _repository;
    public TemperatureLogic(ITemperatureRepository repository)
    {
        _communicator = Communicator.Instance;
        this._repository = repository;
    }
    public async Task<TemperatureReading> GetLatestTemperature(string hardwareId)
    {
        return await _repository.GetLatestTemperature(hardwareId);
    }

    public void saveTemperature(TemperatureReading temperatureReading)
    {
        //repository.update(temperatureReading);
        
    }

    public async Task<ICollection<TemperatureReading>> GetTemperatureHistory(string hardwareId, DateTime dateFrom, DateTime dateTo)
    {
        return await _repository.GetHistory(hardwareId, dateFrom, dateTo);
    }

    public async Task setTemperature(string hardwareId, int level)
    {
        _communicator.setTemperature(hardwareId, level);
    }
    
    public async Task SaveTempReading(string deviceId,double value)
    {
        DateTime dateTime = DateTime.UtcNow;
        await _repository.SaveTemperatureReading(deviceId,value, dateTime);
    }
}