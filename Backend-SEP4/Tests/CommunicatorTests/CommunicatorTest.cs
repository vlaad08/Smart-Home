using Moq;
using Moq.Protected;

namespace Tests.CommunicatorTests;
public class CommunicatorTests
{
    private Communicator _communicator = Communicator.Instance;

    [Fact]
    public void SetTemperature_SendsCorrectMessage()
    {
        string temperature = "5";
        string expectedMessage = $"Set temperature {temperature}";

        var mockCommunicator = new Mock<Communicator>();

        mockCommunicator.CallBase = true;

        _communicator = mockCommunicator.Object;

        _communicator.setTemperature(temperature);

        mockCommunicator.Protected().Verify("Send", Times.Once(), ItExpr.IsAny<string>());
    }
}