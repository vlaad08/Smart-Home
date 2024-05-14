using DBComm.Shared;

namespace DBComm.Logic.Interfaces;

public interface ITemperatureLogic
{
    Task<TemperatureReading> getLatestTemperature(string hardwareId);
    void saveTemperature(TemperatureReading temperatureReading);
    Task<ICollection<TemperatureReading>> getTemperatureHistory(string hardwareId, DateTime dateFrom, DateTime dateTo);
    Task setTemperature(string hardwareId, int level);

}