using DBComm.Logic;
using DBComm.Logic.Interfaces;
using DBComm.Repository;
using Moq;

namespace Tests.NotificationTests;

public class NotificationLogicTest
{
    [Fact]
    public async Task fetchNotifications_calls_dbComm()
    {
        var mock = new Mock<INotificationRepository>();
        var logic = new NotificationLogic(mock.Object);
        logic.GetNotifications("1");
        mock.Verify(d=>d.GetNotifications("1"));
    }

    [Fact]
    public async Task getNotifications_catches_error()
    {
        var mock = new Mock<INotificationRepository>();
        var logic = new NotificationLogic(mock.Object);
        mock.Setup(m => m.GetNotifications("1")).ThrowsAsync(new Exception("test"));
        logic.GetNotifications("1");
        var exception = await Assert.ThrowsAsync<Exception>(() => logic.GetNotifications("1"));
        Assert.Equal("test",exception.Message);
        mock.Verify(m=>m.GetNotifications("1"));
    }

    [Fact]
    public async Task SetBurglarNotification_calls_for_repo()
    {
        var mock = new Mock<INotificationRepository>();
        var logic = new NotificationLogic(mock.Object);
        
        await logic.SetBurglarNotification("1");
        
        mock.Verify(d=>d.AddBurglarNotification("1","BURGLAR IN HOUSE"));
    }
    
    [Fact]
    public async Task SetBurglarNotification_catches_error()
    {
        var mock = new Mock<INotificationRepository>();
        var logic = new NotificationLogic(mock.Object);
        mock.Setup(m => m.AddBurglarNotification("1","BURGLAR IN HOUSE")).ThrowsAsync(new Exception());
        
        var exception = await Assert.ThrowsAsync<Exception>(()=>logic.SetBurglarNotification("1"));
        
        mock.Verify(d=>d.AddBurglarNotification("1","BURGLAR IN HOUSE"),Times.Once);
    }
    
    
}