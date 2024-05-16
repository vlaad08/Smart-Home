using DBComm.Shared;

namespace DBComm.Logic.Interfaces;

public interface ISaveDataLogic
{
    Task saveTempReading(string roomId,double value);
}