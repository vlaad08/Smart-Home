using DBComm.Logic;
using DBComm.Repository;
using Moq;

namespace Tests.TemperatureTests;

public class TemperatureLogicTest
{
  
  [Fact]
  public async void GetTemperature_calls_for_dbcomm()
  {
    var dbComm = new Mock<IBaseRepository>();
    var logic = new TemperatureLogic(dbComm.Object);
    await logic.getLatestTemperature("1");
    dbComm.Verify(d=>d.getOne("1"));
  }
}