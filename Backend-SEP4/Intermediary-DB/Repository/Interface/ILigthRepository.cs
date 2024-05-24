using DBComm.Shared;

namespace DBComm.Repository;

public interface ILigthRepository
{
    Task<LightReading> GetLatestLight(string deviceId);
    Task<ICollection<LightReading>> GetHistory(string deviceId, DateTime dateFrom, DateTime dateTo);
    Task SaveLightReading(string deviceId, double value, DateTime readAt);

}