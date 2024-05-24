using DBComm.Shared;

namespace DBComm.Repository;

public interface ISaveDataRepository
{
    Task<TemperatureReading> SaveTemperatureReading(double value, DateTime readAt);
}