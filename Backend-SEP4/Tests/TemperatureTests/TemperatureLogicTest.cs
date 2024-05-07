using DBComm.Logic;
using DBComm.Repository;
using Moq;

namespace Tests.TemperatureTests;

public class TemperatureLogicTest
{
  
  [Fact]
  public async void GetTemperature_calls_for_dbcomm()
  {
    var dbComm = new Mock<TemperatureRepository>();
    var logic = new TemperatureLogic();
    await logic.getTemperature();
    dbComm.Verify(d=>d.getLatest());
  }
}