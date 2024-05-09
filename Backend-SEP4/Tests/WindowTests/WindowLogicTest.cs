using ConsoleApp1;
using DBComm.Logic;
using DBComm.Logic.Interfaces;
using Moq;

namespace Tests.WindowTests;

public class WindowLogicTest
{
    [Fact]
    public async Task SwithcWindow_logic_calls_for_dbComm()
    {
        var mock = new Mock<ICommunicator>();
        var logic = new WindowLogic(mock.Object);
        await logic.SwitchWindow();
        mock.Verify(m=>m.SwitchWindow());
    }
}