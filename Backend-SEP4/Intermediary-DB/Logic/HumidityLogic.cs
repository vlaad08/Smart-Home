using ConsoleApp1;
using DBComm.Logic.Interfaces;
using DBComm.Repository;
using DBComm.Shared;

namespace DBComm.Logic;

public class HumidityLogic : IHumidityLogic
{
    private ICommunicator communicator;

    private IHumidityRepository _repository;
    //maybe private
    public HumidityLogic(IHumidityRepository repository)
    {
        communicator = Communicator.Instance;
        this._repository = repository;
    }
    
    public async Task<HumidityReading> getHumidity(string hardwareId)
    {
        return await _repository.GetOne(hardwareId);
    }

    public void saveHumidity(Humidity humidity)
    {
        //_repository.update(humidity);
        
    }

    public async Task<ICollection<HumidityReading>> getHumidityHistory(string hardwareId, DateTime dateFrom, DateTime dateTo)
    {
        return await _repository.GetHistory(hardwareId, dateFrom, dateTo);
    }
}