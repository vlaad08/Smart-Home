using ConsoleApp1;
using DBComm.Logic.Interfaces;
using DBComm.Repository;
using DBComm.Shared;

namespace DBComm.Logic;

public class LightLogic : ILightLogic
{
    private ILigthRepository _repository;
    private ICommunicator _communicator;

    public LightLogic(ILigthRepository repository)
    {
        this._repository = repository;
        _communicator = Communicator.Instance;
    }
    public async Task<LightReading> GetLatestLight(string hardwareId)
    {
        return await _repository.GetLatestLight(hardwareId);
    }

    public async Task<ICollection<LightReading>> GetLightHistory(string hardwareId, DateTime dateFrom, DateTime dateTo)
    {
        return await _repository.GetHistory(hardwareId, dateFrom, dateTo);
    }

    public async Task SetLight(string hardwareId, int level)
    {
        _communicator.setLight(hardwareId,level);
    }
    
    public async Task SaveLightReading(string deviceId,double value)
    {
        DateTime readAt = DateTime.UtcNow;
        await _repository.SaveLightReading(deviceId, value, readAt);
    }
}