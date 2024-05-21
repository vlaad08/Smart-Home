using DBComm.Shared;

namespace DBComm.Repository;

public interface ITemperatureRepository
{
    Task<TemperatureReading> GetOne(string deviceId);
    Task<ICollection<TemperatureReading>> GetHistory(string deviceId, DateTime dateFrom, DateTime dateTo);
    Task SaveTemperatureReading(string deviceId,double value, DateTime readAt);
}