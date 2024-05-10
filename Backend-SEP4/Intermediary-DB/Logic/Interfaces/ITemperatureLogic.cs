using DBComm.Shared;

namespace DBComm.Logic.Interfaces;

public interface ITemperatureLogic
{
    Task<Temperature> getTemperature();
    void saveTemperature(Temperature temperature);

}