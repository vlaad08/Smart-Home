using ConsoleApp1;
using DBComm.Logic.Interfaces;

namespace DBComm.Logic;

public class TemperatureLogic : ITemperatureLogic
{
    private ICommunicator communicator;
    //maybe private
    public TemperatureLogic()
    {
        communicator = Communicator.Instance;
    }
    
    public void getTemp()
    {
        Console.WriteLine("segg2");
        communicator.Send("temperature");
    }
}