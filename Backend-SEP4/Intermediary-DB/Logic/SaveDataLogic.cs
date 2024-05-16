using DBComm.Logic.Interfaces;
using DBComm.Repository;
using DBComm.Shared;
using System;
using System.Threading.Tasks;
using ConsoleApp1;

namespace DBComm.Logic
{
    public class SaveDataLogic : ISaveDataLogic
    {
        private readonly ICommunicator _communicator;
        private readonly ISaveDataRepository _repository;

        public SaveDataLogic(ISaveDataRepository repository)
        {
            _communicator = Communicator.Instance ?? throw new ArgumentNullException(nameof(Communicator.Instance));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task saveTempReading(double value)
        {
            DateTime dateTime = DateTime.Now;
            await _repository.SaveTemperatureReading(value, dateTime);
        }
    }
}