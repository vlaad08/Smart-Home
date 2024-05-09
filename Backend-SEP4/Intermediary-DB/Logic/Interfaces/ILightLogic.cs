using DBComm.Shared;

namespace DBComm.Logic.Interfaces;

public interface ILightLogic
{
    Task<LightReading> getLight(string hardwareId);
    Task<ICollection<LightReading>> getLightHistory(string hardwareId, DateTime dateFrom, DateTime dateTo);
    Task SetLight(string hardwareId, int level);
}