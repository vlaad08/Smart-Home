using DBComm.Shared;

namespace DBComm.Logic.Interfaces;

public interface IHumidityLogic
{
    Task<HumidityReading> GetLatestHumidity(string hardwareId);
    Task<ICollection<HumidityReading>> GetHumidityHistory(string hardwareId, DateTime dateFrom, DateTime dateTo);
    Task SaveHumidityReading(string deviceId, double value);
}