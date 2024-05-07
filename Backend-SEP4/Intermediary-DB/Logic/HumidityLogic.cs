using ConsoleApp1;
using DBComm.Logic.Interfaces;
using DBComm.Repository;
using DBComm.Shared;

namespace DBComm.Logic;

public class HumidityLogic : IHumidityLogic
{
    private ICommunicator communicator;

    private IBaseRepository _repository;
    //maybe private
    public HumidityLogic(IBaseRepository repository)
    {
        communicator = Communicator.Instance;
        this._repository = repository;
    }
    
    public async Task<HumidityReading> getHumidity(string hardwareId)
    {
        return (HumidityReading)await _repository.getOne(hardwareId);
    }

    public void saveHumidity(Humidity humidity)
    {
        _repository.update(humidity);
        
    }
}