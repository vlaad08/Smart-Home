using DBComm.Shared;

namespace DBComm.Logic.Interfaces;

public interface ITemperatureLogic
{
    Task<TemperatureReading> getLatestTemperature(string hardwareId);
    void saveTemperature(TemperatureReading temperatureReading);

}