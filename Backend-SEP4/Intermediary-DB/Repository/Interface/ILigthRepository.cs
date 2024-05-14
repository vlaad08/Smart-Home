using DBComm.Shared;

namespace DBComm.Repository;

public interface ILigthRepository
{
    Task<LightReading> GetOne(string deviceId);
    Task<ICollection<LightReading>> GetHistory(string deviceId, DateTime dateFrom, DateTime dateTo);
}