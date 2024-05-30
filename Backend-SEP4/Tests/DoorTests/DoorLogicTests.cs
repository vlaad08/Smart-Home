using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using DBComm.Logic;
using DBComm.Repository;
using Moq;

namespace Tests.DoorTests;

public class DoorLogicTest
{
    private const string ServerIp = "127.0.0.1";
    private const int ServerPort = 6868;

    private TcpListener _server;
    private Task _serverTask;
    private CancellationTokenSource _cancellationTokenSource;

    private async Task<string> hash(String input)
    {
        byte[] inputBytes = Encoding.UTF8.GetBytes(input);
        string hashedString;
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(inputBytes);
            hashedString = BitConverter.ToString(hashBytes).Replace("-", "");
        }
        return hashedString;
    }

    private async Task StartServer()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        _server = new TcpListener(IPAddress.Parse(ServerIp), ServerPort);
        _server.Start();
        _serverTask = Task.Run(() => ServerLoop(_cancellationTokenSource.Token), _cancellationTokenSource.Token);
        await Task.Delay(100); // Ensure the server has time to start
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
     public async Task ChangeLockPassword_ValidInput_hashing_works()
     {
         await StartServer();
         try
         {
             var mock = new Mock<IDoorRepository>();
             TcpClient c = new TcpClient(ServerIp, ServerPort);
             var logic = new DoorLogic(mock.Object, c);
             mock.Setup(m => m.CheckIfDoorExist("1")).ReturnsAsync(true);
             byte[] inputBytes = Encoding.UTF8.GetBytes("12345678");
             string hashedString;
             using (SHA256 sha256 = SHA256.Create())
             {
                 byte[] hashBytes = sha256.ComputeHash(inputBytes);
                 hashedString = BitConverter.ToString(hashBytes).Replace("-", "");
             }
             await logic.ChangeLockPassword("1", "12345678");
             
             mock.Verify(m=>m.ChangePassword("1",hashedString));
             
             
         }
         finally
         {
             await StopServer();
         }
     }
     
     [Fact]
     public async Task ChangeLockPassword_EmptyHomeId_ThrowsException()
     {
         await StartServer();
         try
         {
             var mock = new Mock<IDoorRepository>();
             TcpClient c = new TcpClient(ServerIp, ServerPort);
             var logic = new DoorLogic(mock.Object, c);
             
             var exception = await Assert.ThrowsAsync<Exception>(()=>logic.ChangeLockPassword(null, "12345678"));
             
             mock.Verify(m=>m.ChangePassword("1",It.IsAny<string>()),Times.Never);
             Assert.Equal("House Id can not be empty",exception.Message);
             
         }
         finally
         {
             await StopServer();
         }
     }
     [Fact]
     public async Task ChangeLockPassword_DoorDoesNotExist()
     {
         await StartServer();
         try
         {
             var mock = new Mock<IDoorRepository>();
             TcpClient c = new TcpClient(ServerIp, ServerPort);
             var logic = new DoorLogic(mock.Object, c);
             mock.Setup(m => m.CheckIfDoorExist("1")).ThrowsAsync(new Exception($"Door does not exist."));
             
             var exception = await Assert.ThrowsAsync<Exception>(()=>logic.ChangeLockPassword("1", "12345678"));
             
             mock.Verify(m=>m.ChangePassword("1",It.IsAny<string>()),Times.Never);
             Assert.Equal($"Door does not exist.",exception.Message);
             
         }
         finally
         {
             await StopServer();
         }
     }

     [Fact]
     public async Task SwitchDoor_calls_for_repo()
     {
         await StartServer();
         try
         {
             var mock = new Mock<IDoorRepository>();
             TcpClient c = new TcpClient(ServerIp, ServerPort);
             var logic = new DoorLogic(mock.Object, c);
             mock.Setup(m => m.CheckIfDoorExist("1")).ThrowsAsync(new Exception($"Door does not exist."));
             string h = await hash("12345678");
             mock.Setup(m => m.CheckPassword("1", "12345678")).ReturnsAsync(h);
             mock.Setup(m => m.GetFirstDeviceInHouse("1")).ReturnsAsync("1");

             logic.SwitchDoor("1", "12345678", true);
             
             mock.Verify(m=>m.SaveDoorState("1",true));
         }
         finally
         {
             await StopServer();
         }
     }
     
     [Fact]
     public async Task SwitchDoor_calls_for_stream_and_sends_message()
     {
         await StartServer();
         try
         {
             var mock = new Mock<IDoorRepository>();
             TcpClient c = new TcpClient(ServerIp, ServerPort);
             var logic = new DoorLogic(mock.Object, c);
             var clientField = typeof(DoorLogic).GetField("client", BindingFlags.NonPublic | BindingFlags.Instance);
             clientField.SetValue(logic, c);
             var streamField = typeof(DoorLogic).GetField("stream", BindingFlags.NonPublic | BindingFlags.Instance);
             NetworkStream s = c.GetStream();
             streamField.SetValue(logic, s);
             mock.Setup(m => m.CheckIfDoorExist("1")).ThrowsAsync(new Exception($"Door does not exist."));
             string h = await hash("12345678");
             mock.Setup(m => m.CheckPassword("1", "12345678")).ReturnsAsync(h);
             mock.Setup(m => m.GetFirstDeviceInHouse("1")).ReturnsAsync("1");

             await logic.SwitchDoor("1", "12345678", true);
             
             Assert.True(logic.writeAsyncCalled,"Write on stream hasn't been called");
         }
         finally
         {
             await StopServer();
         }
     }

     [Fact]
     public async Task SwitchDoor_throws_errors_up()
     {
         await StartServer();
         try
         {
             var mock = new Mock<IDoorRepository>();
             TcpClient c = new TcpClient(ServerIp, ServerPort);
             var logic = new DoorLogic(mock.Object, c);
             string h = await hash("12345678");
             mock.Setup(m => m.CheckPassword("1", "12345678")).ReturnsAsync(h);
             mock.Setup(m => m.GetFirstDeviceInHouse("1"))
                 .ThrowsAsync(new Exception($"No devices found in the house with ID 1."));

             var exception = await Assert.ThrowsAsync<Exception>(() => logic.SwitchDoor("1", "12345678", true));
         
             mock.Verify(m=>m.SaveDoorState("1",true),Times.Never);
             Assert.Equal($"No devices found in the house with ID 1.",exception.Message);
         }
         finally
         {
             await StopServer();
         }
     }
     
     [Fact]
     public async Task SwitchDoor_sends_notification()
     {
         await StartServer();
         try
         {
             var mock = new Mock<IDoorRepository>();
             var notifMock = new Mock<INotificationRepository>();
             TcpClient c = new TcpClient(ServerIp, ServerPort);
             var logic = new DoorLogic(mock.Object,notifMock.Object, c);
             
             mock.Setup(m => m.CheckIfDoorExist("1")).ThrowsAsync(new Exception($"Door does not exist."));
             string h = await hash("12345678");
             mock.Setup(m => m.CheckPassword("1", "12345678")).ReturnsAsync("random string that will never be the hash of the password");
             mock.Setup(m => m.GetFirstDeviceInHouse("1")).ReturnsAsync("1");

             await Assert.ThrowsAsync<Exception>(()=>logic.SwitchDoor("1","12345678",true));
             
             mock.Verify(m=>m.SaveDoorState("1",true),Times.Never);
             notifMock.Verify(m=>m.AddNotification("1","Someone entered wrong password to your door!"));
         }
         finally
         {
             await StopServer();
         }
     }
     
     [Fact]
     public async Task ChangeLockPassword_throws_errors_up()
     {
         await StartServer();
         try
         {
             var mock = new Mock<IDoorRepository>();
             TcpClient c = new TcpClient(ServerIp, ServerPort);
             var logic = new DoorLogic(mock.Object, c);

             var exception = await Assert.ThrowsAsync<Exception>(() => logic.ChangeLockPassword(null, "12345678"));
         
             mock.Verify(m=>m.ChangePassword(null,It.IsAny<string>()),Times.Never);
             Assert.Equal("House Id can not be empty",exception.Message);
         }
         finally
         {
             await StopServer();
         }
     }
     
     [Fact]
     public async Task ChangeLockPassword_calls_for_repo()
     {
         await StartServer();
         try
         {
             var mock = new Mock<IDoorRepository>();
             TcpClient c = new TcpClient(ServerIp, ServerPort);
             var logic = new DoorLogic(mock.Object, c);
             string h = await hash("11111111");
             mock.Setup(m => m.CheckIfDoorExist("1")).ReturnsAsync(true);
             
             await logic.ChangeLockPassword("1", "11111111");

             mock.Verify(m => m.ChangePassword("1", h));
         }
         finally
         {
             await StopServer();
         }
     }

     [Fact]
     public async Task GetDoorState_calls_for_repo()
     {
         await StartServer();
         try
         {
             var mock = new Mock<IDoorRepository>();
             TcpClient c = new TcpClient(ServerIp, ServerPort);
             var logic = new DoorLogic(mock.Object, c);

             await logic.GetDoorState("1");
             
             mock.Verify(m=>m.CheckDoorState("1"));
         }
         finally
         {
             await StopServer();
         }
     }
     
     [Fact]
     public async Task GetDoorState_throws_error()
     {
         await StartServer();
         try
         {
             var mock = new Mock<IDoorRepository>();
             TcpClient c = new TcpClient(ServerIp, ServerPort);
             var logic = new DoorLogic(mock.Object, c);
             mock.Setup(m => m.CheckDoorState("1")).ThrowsAsync(new Exception());

             await Assert.ThrowsAsync<Exception>(() => logic.GetDoorState("1"));
             mock.Verify(m=>m.CheckDoorState("1"));
         }
         finally
         {
             await StopServer();
         }
     }
}