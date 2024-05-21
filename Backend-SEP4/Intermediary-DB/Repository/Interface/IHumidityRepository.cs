using DBComm.Shared;

namespace DBComm.Repository;

public interface IHumidityRepository
{
    Task<HumidityReading> GetOne(string deviceId);
    Task<ICollection<HumidityReading>> GetHistory(string deviceId, DateTime dateFrom, DateTime dateTo);
    Task SaveHumidityReading(string deviceId, double value, DateTime readAt);

}