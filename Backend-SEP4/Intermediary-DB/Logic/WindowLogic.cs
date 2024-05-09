using ConsoleApp1;
using DBComm.Logic.Interfaces;

namespace DBComm.Logic;

public class WindowLogic : IWindowLogic
{
    private ICommunicator _communicator;

    public WindowLogic()
    {
        _communicator = Communicator.Instance;
    }

    public async Task SwitchWindow()
    {
        await _communicator.SwitchWindow();
    }
}