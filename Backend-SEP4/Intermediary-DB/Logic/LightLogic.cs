using DBComm.Logic.Interfaces;
using DBComm.Repository;
using DBComm.Shared;

namespace DBComm.Logic;

public class LightLogic : ILightLogic
{
    private IBaseRepository _repository;

    public LightLogic(IBaseRepository repository)
    {
        this._repository = repository;
    }
    public async Task<LightReading> getLight(string hardwareId)
    {
        return (LightReading)await _repository.getOne(hardwareId);
    }
}