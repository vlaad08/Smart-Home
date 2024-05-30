using System.Reflection;
using System.Runtime.InteropServices.JavaScript;
using DBComm.Logic;
using DBComm.Repository;
using DBComm.Shared;
using Moq;
using WebAPI.DTOs;

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

    [Fact]
    public async Task SaveHumidityReading_calls_for_repository()
    {
        RoomDataDTO dto = new RoomDataDTO()
         {
             DeviceId = "2",
             Home = new Home("1"),
             HumiValue = 40,
             Id = "2",
             IsWindowOpen = true,
             LightLevel = 3,
             LightValue = 4,
             Name = "1",
             PreferedHumidity = 40,
             PreferedTemperature = 23
         };
        var dbComm = new Mock<IHumidityRepository>();
        var roomMock = new Mock<IRoomRepository>();
        var notifMock = new Mock<INotificationRepository>();
        roomMock.Setup(r => r.GetRoomData(null, "1", false, false, false)).ReturnsAsync(dto);
        var logic = new HumidityLogic(dbComm.Object);
        var roomRepoField =
            typeof(HumidityLogic).GetField("_roomRepository", BindingFlags.NonPublic | BindingFlags.Instance);
        roomRepoField.SetValue(logic,roomMock.Object);
        var notificationRepoField =  typeof(HumidityLogic).GetField("_notificationRepository", BindingFlags.NonPublic | BindingFlags.Instance);
        notificationRepoField.SetValue(logic,notifMock.Object);
        
        await logic.SaveHumidityReading("1",30);
        
        dbComm.Verify(d=>d.SaveHumidityReading("1",30 ,It.IsAny<DateTime>()));
    }
}