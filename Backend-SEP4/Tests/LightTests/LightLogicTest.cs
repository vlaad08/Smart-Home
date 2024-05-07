using DBComm.Logic;
using DBComm.Repository;
using Moq;

namespace Tests.TemperatureTests;

public class LightLogicTest
{
  
  [Fact]
  public async void GetLght_calls_for_dbcomm()
  {
    var dbComm = new Mock<LightRepository>();
    var logic = new LightLogic();
    await logic.getLight();
    dbComm.Verify(d=>d.getLatest());
  }
}