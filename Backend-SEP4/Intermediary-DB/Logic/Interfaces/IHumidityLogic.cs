using DBComm.Shared;

namespace DBComm.Logic.Interfaces;

public interface IHumidityLogic
{
    Task<HumidityReading> getHumidity(string hardwareId);
    void saveHumidity(Humidity humidity);
}