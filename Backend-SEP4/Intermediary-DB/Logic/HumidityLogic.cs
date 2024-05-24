using ConsoleApp1;
using DBComm.Logic.Interfaces;
using DBComm.Repository;
using DBComm.Shared;

namespace DBComm.Logic;

public class HumidityLogic : IHumidityLogic
{
    private ICommunicator communicator;

    private IHumidityRepository _repository;
    public HumidityLogic(IHumidityRepository repository)
    {
        communicator = Communicator.Instance;
        this._repository = repository;
    }
    
    public async Task<HumidityReading> GetLatestHumidity(string hardwareId)
    {
        return await _repository.GetLatestHumidity(hardwareId);
    }
    public async Task<ICollection<HumidityReading>> GetHumidityHistory(string hardwareId, DateTime dateFrom, DateTime dateTo)
    {
        return await _repository.GetHistory(hardwareId, dateFrom, dateTo);
    }

    public async Task SaveHumidityReading(string deviceId, double value)
    {
        DateTime dateTime = DateTime.UtcNow;
        await _repository.SaveHumidityReading(deviceId, value, dateTime);
    }
}