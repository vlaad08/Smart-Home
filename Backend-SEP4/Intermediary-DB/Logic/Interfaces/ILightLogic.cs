using DBComm.Shared;

namespace DBComm.Logic.Interfaces;

public interface ILightLogic
{
    Task<LightReading> GetLatestLight(string hardwareId);
    Task<ICollection<LightReading>> GetLightHistory(string hardwareId, DateTime dateFrom, DateTime dateTo);
    Task SaveLightReading(string deviceId, double value);

}