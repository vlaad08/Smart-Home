using DBComm.Logic.Interfaces;
using DBComm.Repository;
using DBComm.Shared;

namespace DBComm.Logic;

public class LightLogic : ILightLogic
{
    private ILigthRepository _repository;

    public LightLogic(ILigthRepository repository)
    {
        this._repository = repository;
    }
    public async Task<LightReading> getLight(string hardwareId)
    {
        return await _repository.GetOne(hardwareId);
    }

    public async Task<ICollection<LightReading>> getLightHistory(string hardwareId, DateTime dateFrom, DateTime dateTo)
    {
        return await _repository.GetHistory(hardwareId, dateFrom, dateTo);
    }
}