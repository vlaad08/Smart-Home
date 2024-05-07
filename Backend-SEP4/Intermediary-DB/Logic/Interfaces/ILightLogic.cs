using DBComm.Shared;

namespace DBComm.Logic.Interfaces;

public interface ILightLogic
{
    Task<LightReading> getLight(string hardwareId);
}