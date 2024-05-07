using DBComm.Logic.Interfaces;
using DBComm.Repository;
using DBComm.Shared;

namespace DBComm.Logic;

public class LightLogic : ILightLogic
{
    private LightRepository _repository;

    public LightLogic()
    {
        this._repository = new LightRepository();
    }
    public async Task<Light> getLight()
    {
        return await _repository.getLatest();
    }
}