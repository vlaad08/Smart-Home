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
        logic.GetNotifications();
        mock.Verify(d=>d.GetNotifications());
    }
}