using DBComm.Shared;

namespace DBComm.Logic.Interfaces;

public interface IHumidityLogic
{
    Task<Humidity> getHumidity();
    void saveHumidity(Humidity humidity);
}