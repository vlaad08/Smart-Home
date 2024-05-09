using DBComm.Logic;
using DBComm.Repository;
using Moq;

namespace Tests.TemperatureTests;

public class TemperatureLogicTest
{
  
  [Fact]
  public async void GetTemperature_calls_for_dbcomm()
  {
    var dbComm = new Mock<ITemperatureRepository>();
    var logic = new TemperatureLogic(dbComm.Object);
    await logic.getLatestTemperature("1");
    dbComm.Verify(d=>d.GetOne("1"));
  }
}