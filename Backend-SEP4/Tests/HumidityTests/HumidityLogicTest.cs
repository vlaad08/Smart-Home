using DBComm.Logic;
using DBComm.Repository;
using Moq;

namespace Tests.HumidityTests;

public class HumidityLogicTest
{
    [Fact]
    public async void GetHumidity_calls_for_dbcomm()
    {
        var dbComm = new Mock<HumidityRepository>();
        var logic = new HumidityLogic();
        await logic.getHumidity();
        dbComm.Verify(d=>d.getLates());
    }
}