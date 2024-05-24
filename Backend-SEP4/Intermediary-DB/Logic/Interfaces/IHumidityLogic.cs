using DBComm.Shared;

namespace DBComm.Logic.Interfaces;

public interface IHumidityLogic
{
    Task<HumidityReading> getHumidity(string hardwareId);
    void saveHumidity(Humidity humidity);
    Task<ICollection<HumidityReading>> getHumidityHistory(string hardwareId, DateTime dateFrom, DateTime dateTo);
    Task saveHumidityReading(string deviceId, double value);
}