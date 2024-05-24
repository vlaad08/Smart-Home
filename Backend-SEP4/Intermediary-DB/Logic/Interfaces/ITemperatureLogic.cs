using DBComm.Shared;

namespace DBComm.Logic.Interfaces;

public interface ITemperatureLogic
{
    Task<TemperatureReading> GetLatestTemperature(string hardwareId);
    Task<ICollection<TemperatureReading>> GetTemperatureHistory(string hardwareId, DateTime dateFrom, DateTime dateTo);
    Task SaveTempReading(string deviceId,double value);
    Task SetTemperature(string hardwareId, int level);

}