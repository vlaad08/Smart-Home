using System.Runtime.InteropServices.JavaScript;
using DBComm.Logic;
using DBComm.Repository;
using Moq;

namespace Tests.HumidityTests;

public class HumidityLogicTest
{
    [Fact]
    public async void GetHumidity_calls_for_dbcomm()
    {
        var dbComm = new Mock<IHumidityRepository>();
        var logic = new HumidityLogic(dbComm.Object);
        await logic.GetLatestHumidity("1");
        dbComm.Verify(d=>d.GetLatestHumidity("1"));
    }

    [Fact]
    public async Task GetHumidityHistory_calls_for_repository()
    {
        var dbComm = new Mock<IHumidityRepository>();
        var logic = new HumidityLogic(dbComm.Object);
        var now1 = DateTime.Now;
        var now2 = DateTime.Now;
        await logic.GetHumidityHistory("1",now1 ,now2);
        dbComm.Verify(d=>d.GetHistory("1",now1 ,now2));
    }

    // [Fact]
    // public async Task SaveHumidityReading_calls_for_repository()
    // {
    //     var dbComm = new Mock<IHumidityRepository>();
    //     var logic = new HumidityLogic(dbComm.Object);
    //     await logic.SaveHumidityReading("1",50);
    //     dbComm.Verify(d=>d.SaveHumidityReading("1",50 ,It.IsAny<DateTime>()));
    // }
}