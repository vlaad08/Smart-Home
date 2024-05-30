using System.Net;
using System.Net.Sockets;
using System.Reflection;
using Castle.Components.DictionaryAdapter;
using DBComm.Logic;
using DBComm.Repository;
using DBComm.Shared;
using Moq;
using WebAPI.DTOs;

namespace Tests.TemperatureTests;

public class TemperatureLogicTest
{
 
  private const string ServerIp = "127.0.0.1";
      private const int ServerPort = 6868;
  
      private TcpListener _server;
      private Task _serverTask;
      private CancellationTokenSource _cancellationTokenSource;
      
      private async Task StartServer()
      {
          _cancellationTokenSource = new CancellationTokenSource();
          _server = new TcpListener(IPAddress.Parse(ServerIp), ServerPort);
          _server.Start();
          _serverTask = Task.Run(() => ServerLoop(_cancellationTokenSource.Token), _cancellationTokenSource.Token);
          await Task.Delay(1000); // Ensure the server has time to start
      }
  
      private async Task StopServer()
      {
          if (_server != null)
          {
              _cancellationTokenSource.Cancel();
              _server.Stop();
              _server = null;
          }
          if (_serverTask != null)
          {
              try
              {
                  await _serverTask;
              }
              catch (OperationCanceledException)
              {
                  // Expected exception on cancellation
              }
              _serverTask = null;
          }
      }
  
      private async Task ServerLoop(CancellationToken token)
      {
          while (!token.IsCancellationRequested)
          {
              if (_server.Pending())
              {
                  var client = await _server.AcceptTcpClientAsync();
                  using var stream = client.GetStream();
                  byte[] buffer = new byte[256];
                  int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, token);
                  break; 
              }
              await Task.Delay(100, token); 
          }
      }
  
      [Fact]
      public async void GetTemperature_calls_for_repo()
      {
          await StartServer();
          try
          {
              var mock = new Mock<ITemperatureRepository>();
              TcpClient c = new TcpClient(ServerIp, ServerPort);
              var logic = new TemperatureLogic(mock.Object, c);
              await logic.GetLatestTemperature("1");
              mock.Verify(d=>d.GetLatestTemperature("1"));
          }
          finally
          {
              await StopServer();
          }
      }

      [Fact]
      public async Task GetTemperatureHistory_calls_for_repo()
      {
          await StartServer();
          try
          {
              var mock = new Mock<ITemperatureRepository>();
              TcpClient c = new TcpClient(ServerIp, ServerPort);
              var logic = new TemperatureLogic(mock.Object, c);
              DateTime now1 = DateTime.Now;
              DateTime now2 = DateTime.Now;
              
              await logic.GetTemperatureHistory("1",now1,now2);
              
              mock.Verify(d=>d.GetHistory("1",now1,now2));
          }
          finally
          {
              await StopServer();
          }
      }

      [Fact]
      public async Task SetTemperature_calls_for_stream_and_sends_message()
      {
          await StartServer();
          try
          {
              var mock = new Mock<ITemperatureRepository>();
              TcpClient c = new TcpClient(ServerIp, ServerPort);
              var logic = new TemperatureLogic(mock.Object, c);
              var clientField =
                  typeof(TemperatureLogic).GetField("client", BindingFlags.NonPublic | BindingFlags.Instance);
              clientField.SetValue(logic, c);
              var streamField =
                  typeof(TemperatureLogic).GetField("stream", BindingFlags.NonPublic | BindingFlags.Instance);
              NetworkStream s = c.GetStream();
              streamField.SetValue(logic, s);

              await logic.SetTemperature("1", 2);

              Assert.True(logic.writeAsyncCalled, "Write to stream didn't happen");
          }
          finally
          {
              await StopServer();
          }
      }

      [Fact]
      public async Task SetTemperature_throws_exception()
      {
          await StartServer();
          try{
              
              var mock = new Mock<ITemperatureRepository>();
              TcpClient c = new TcpClient(ServerIp, ServerPort);
              var logic = new TemperatureLogic(mock.Object, c);
              var clientField =
                  typeof(TemperatureLogic).GetField("client", BindingFlags.NonPublic | BindingFlags.Instance);
              clientField.SetValue(logic, c);
              var streamField =
                  typeof(TemperatureLogic).GetField("stream", BindingFlags.NonPublic | BindingFlags.Instance);
              NetworkStream s = c.GetStream();
              streamField.SetValue(logic, s);

              var exception = await Assert.ThrowsAsync<Exception>(()=> logic.SetTemperature("1", 7));

              Assert.Equal("Level must be between 1 and 6.",exception.Message);
          }
          finally
          {
              await StopServer();
          }
      }
      

      [Fact]
      public async Task SaveTempReading_calls_for_repo()
      {
          await StartServer();
          try
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

              var mock = new Mock<ITemperatureRepository>();
              var roomMock = new Mock<IRoomRepository>();
              roomMock.Setup(r => r.GetRoomData(null, "1", false, false, false)).ReturnsAsync(dto);

              TcpClient c = new TcpClient(ServerIp, ServerPort);
              var logic = new TemperatureLogic(mock.Object, c);
              var clientField = typeof(TemperatureLogic).GetField("client", BindingFlags.NonPublic | BindingFlags.Instance);
              clientField.SetValue(logic, c);
              var streamField = typeof(TemperatureLogic).GetField("stream", BindingFlags.NonPublic | BindingFlags.Instance);
              NetworkStream s = c.GetStream();
              streamField.SetValue(logic, s);

              var roomRepositoryField = typeof(TemperatureLogic).GetField("_roomRepository", BindingFlags.NonPublic | BindingFlags.Instance);
              roomRepositoryField.SetValue(logic, roomMock.Object);

              await logic.SaveTempReading("1", 22);
              
              mock.Verify(m => m.SaveTemperatureReading("1", 22, It.IsAny<DateTime>()));
          }
          catch (Exception ex)
          {
              Console.WriteLine(ex);
              throw;
          }
          finally
          {
              await StopServer();
          }
      }
      
      [Fact]
      public async Task SaveTempReading_throws_on_null_dto_home()
      {
          await StartServer();
          try
          {
              RoomDataDTO dto = new RoomDataDTO()
              {

                  DeviceId = "2",
                  Home = null,
                  HumiValue = 40,
                  Id = "2",
                  IsWindowOpen = true,
                  LightLevel = 3,
                  LightValue = 4,
                  Name = "1",
                  PreferedHumidity = 40,
                  PreferedTemperature = 23

              };

              var mock = new Mock<ITemperatureRepository>();
              var roomMock = new Mock<IRoomRepository>();
              roomMock.Setup(r => r.GetRoomData(null, "1", false, false, false)).ReturnsAsync(dto);

              TcpClient c = new TcpClient(ServerIp, ServerPort);
              var logic = new TemperatureLogic(mock.Object, c);
              var clientField = typeof(TemperatureLogic).GetField("client", BindingFlags.NonPublic | BindingFlags.Instance);
              clientField.SetValue(logic, c);
              var streamField = typeof(TemperatureLogic).GetField("stream", BindingFlags.NonPublic | BindingFlags.Instance);
              NetworkStream s = c.GetStream();
              streamField.SetValue(logic, s);

              var roomRepositoryField = typeof(TemperatureLogic).GetField("_roomRepository", BindingFlags.NonPublic | BindingFlags.Instance);
              roomRepositoryField.SetValue(logic, roomMock.Object);

              var exception = await Assert.ThrowsAsync<InvalidOperationException>(()=> logic.SaveTempReading("1", 22));
              
              mock.Verify(m => m.SaveTemperatureReading("1", 22, It.IsAny<DateTime>()),Times.Never);
              Assert.Equal("Home data is null.",exception.Message);
          }
          catch (Exception ex)
          {
              Console.WriteLine(ex);
              throw;
          }
          finally
          {
              await StopServer();
          }
      }
      
      [Fact]
      public async Task SaveTempReading_throws_on_null_dto()
      {
          await StartServer();
          try
          {
              RoomDataDTO dto = null;

              var mock = new Mock<ITemperatureRepository>();
              var roomMock = new Mock<IRoomRepository>();
              roomMock.Setup(r => r.GetRoomData(null, "1", false, false, false)).ReturnsAsync(dto);

              TcpClient c = new TcpClient(ServerIp, ServerPort);
              var logic = new TemperatureLogic(mock.Object, c);
              var clientField = typeof(TemperatureLogic).GetField("client", BindingFlags.NonPublic | BindingFlags.Instance);
              clientField.SetValue(logic, c);
              var streamField = typeof(TemperatureLogic).GetField("stream", BindingFlags.NonPublic | BindingFlags.Instance);
              NetworkStream s = c.GetStream();
              streamField.SetValue(logic, s);

              var roomRepositoryField = typeof(TemperatureLogic).GetField("_roomRepository", BindingFlags.NonPublic | BindingFlags.Instance);
              roomRepositoryField.SetValue(logic, roomMock.Object);

              var exception = await Assert.ThrowsAsync<InvalidOperationException>(()=> logic.SaveTempReading("1", 22));
              
              mock.Verify(m => m.SaveTemperatureReading("1", 22, It.IsAny<DateTime>()),Times.Never);
              Assert.Equal("Room data is null.",exception.Message);
          }
          catch (Exception ex)
          {
              Console.WriteLine(ex);
              throw;
          }
          finally
          {
              await StopServer();
          }
      }
      
      [Fact]
    public async Task SaveTempReading_sends_notification_on_lower_preferred_temp()
    {
        await StartServer();
        try
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

            var mock = new Mock<ITemperatureRepository>();
            var roomMock = new Mock<IRoomRepository>();
            var notifMock = new Mock<INotificationRepository>();
            roomMock.Setup(r => r.GetRoomData(null, "1",false,false,false)).ReturnsAsync(dto);
            notifMock.Setup(n => n.GetNotifications(It.IsAny<string>())).ReturnsAsync((List<Notification>)null);

            TcpClient c = new TcpClient(ServerIp, ServerPort);
            var logic = new TemperatureLogic(mock.Object, c);
            var clientField = typeof(TemperatureLogic).GetField("client", BindingFlags.NonPublic | BindingFlags.Instance);
            clientField.SetValue(logic, c);
            var streamField = typeof(TemperatureLogic).GetField("stream", BindingFlags.NonPublic | BindingFlags.Instance);
            NetworkStream s = c.GetStream();
            streamField.SetValue(logic, s);
            var roomRepositoryField = typeof(TemperatureLogic).GetField("_roomRepository", BindingFlags.NonPublic | BindingFlags.Instance);
            roomRepositoryField.SetValue(logic, roomMock.Object);
            var notificationRepositoryField = typeof(TemperatureLogic).GetField("_notificationRepository", BindingFlags.NonPublic | BindingFlags.Instance);
            notificationRepositoryField.SetValue(logic, notifMock.Object);

            await logic.SaveTempReading("1", 24);

            mock.Verify(m => m.SaveTemperatureReading("1", 24, It.IsAny<DateTime>()), Times.Once);
            notifMock.Verify(m => m.AddNotification("1", "The temperature in 1 is higher than preferred and it is 24 degrees"), Times.Once);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
        finally
        {
            await StopServer();
        }
    }
    
//     [Fact]
// public async Task SaveTempReading_gets_notification_for_room()
// {
//     await StartServer();
//     try
//     {
//         RoomDataDTO dto = new RoomDataDTO()
//         {
//             DeviceId = "2",
//             Home = new Home("1"),
//             HumiValue = 40,
//             Id = "2",
//             IsWindowOpen = true,
//             LightLevel = 3,
//             LightValue = 4,
//             Name = "1",
//             PreferedHumidity = 40,
//             PreferedTemperature = 23
//         };
//         
//         List<Notification> notifications = new List<Notification>
//         {
//             new Notification()
//             {
//                 Home = new Home("1"),
//                 Id = "1",
//                 Message = "The temperature in 1 is higher than preferred",
//                 SendAt = DateTime.Now
//             }
//         };
//         
//         var mock = new Mock<ITemperatureRepository>();
//         var roomMock = new Mock<IRoomRepository>();
//         var notifMock = new Mock<INotificationRepository>();
//         
//         roomMock.Setup(r => r.GetRoomData(null, "1",false,false,false)).ReturnsAsync(dto);
//         notifMock.Setup(n => n.GetNotifications("1")).ReturnsAsync(notifications);
//
//         TcpClient c = new TcpClient(ServerIp, ServerPort);
//         var logic = new TemperatureLogic(mock.Object, c);
//         var clientField = typeof(TemperatureLogic).GetField("client", BindingFlags.NonPublic | BindingFlags.Instance);
//         clientField.SetValue(logic, c);
//         var streamField = typeof(TemperatureLogic).GetField("stream", BindingFlags.NonPublic | BindingFlags.Instance);
//         NetworkStream s = c.GetStream();
//         streamField.SetValue(logic, s);
//         var roomRepositoryField = typeof(TemperatureLogic).GetField("_roomRepository", BindingFlags.NonPublic | BindingFlags.Instance);
//         roomRepositoryField.SetValue(logic, roomMock.Object);
//         var notificationRepositoryField = typeof(TemperatureLogic).GetField("_notificationRepository", BindingFlags.NonPublic | BindingFlags.Instance);
//         notificationRepositoryField.SetValue(logic, notifMock.Object);
//
//         await logic.SaveTempReading("1", 24);
//
//         mock.Verify(m => m.SaveTemperatureReading("1", 24, It.IsAny<DateTime>()), Times.Once);
//         notifMock.Verify(m => m.AddNotification("1", "The temperature in 1 is higher than preferred and it is 24 degrees"), Times.Once);
//     }
//     catch (Exception ex)
//     {
//         Console.WriteLine(ex);
//         throw;
//     }
//     finally
//     {
//         await StopServer();
//     }
// }



      
      
}