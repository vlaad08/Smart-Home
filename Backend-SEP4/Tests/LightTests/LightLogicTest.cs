using System.Reflection;
using ConsoleApp1;
using DBComm.Logic;
using DBComm.Logic.Interfaces;
using DBComm.Repository;
using Moq;

namespace Tests.TemperatureTests;

public class LightLogicTest
{
  

  [Fact]
  public async void GetLight_calls_for_dbcomm()
  {
    var dbComm = new Mock<ILigthRepository>();
    var logic = new LightLogic(dbComm.Object);
    await logic.GetLatestLight("1");
    dbComm.Verify(d=>d.GetLatestLight("1"));
  }

  [Fact]
  public async Task GetLightHistory_calls_for_repository()
  {
    var dbComm = new Mock<ILigthRepository>();
    var logic = new LightLogic(dbComm.Object);
    var now1 = DateTime.Now;
    var now2 = DateTime.Now;
    await logic.GetLightHistory("1",now1,now2);
    dbComm.Verify(d=>d.GetHistory("1",now1,now2));
  }
  
  [Fact]
  public async Task SetLight_calls_for_communicator()
  {
    var dbComm = new Mock<ILigthRepository>();
    var communicatorMock = new Mock<ICommunicator>();

    var logic = new LightLogic(dbComm.Object);

    var communicatorField = typeof(LightLogic).GetField("_communicator", BindingFlags.NonPublic | BindingFlags.Instance);
    communicatorField.SetValue(logic, communicatorMock.Object);

    await logic.SetLight("1", 1);

    communicatorMock.Verify(c => c.setLight("1", 1), Times.Once);
  }

  [Fact]
  public async Task savLightReading_calls_for_repository()
  {
    var dbComm = new Mock<ILigthRepository>();
    var logic = new LightLogic(dbComm.Object);
    logic.SaveLightReading("1", 3);
    dbComm.Verify(m=>m.SaveLightReading("1",3,It.IsAny<DateTime>()));
  }

}