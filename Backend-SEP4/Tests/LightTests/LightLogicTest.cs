using DBComm.Logic;
using DBComm.Repository;
using Moq;

namespace Tests.TemperatureTests;

public class LightLogicTest
{
  
  [Fact]
  public async void GetLight_calls_for_dbcomm()
  {
    var dbComm = new Mock<IBaseRepository>();
    var logic = new LightLogic(dbComm.Object);
    await logic.getLight("1");
    dbComm.Verify(d=>d.getOne("1"));
  }
}