using DBComm.Shared;

namespace DBComm.Logic.Interfaces;

public interface ILightLogic
{
    Task<Light> getLight();
}