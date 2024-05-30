using Moq;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using DBComm.Logic;
using DBComm.Repository;
using ECC;
using ECC.Interface;
using Xunit;

public class RoomLogicTests
{
    private const string ServerIp = "127.0.0.1";
    private const int ServerPort = 6868;

    private TcpListener _server;
    private Task _serverTask;
    private CancellationTokenSource _cancellationTokenSource;
    private bool _writeAsyncCalled;

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
                _writeAsyncCalled = true;
                break; 
            }
            await Task.Delay(100, token); 
        }
    }

    
    
    [Fact]
    public async Task AddRoom_calls_for_repository()
    {
        await StartServer();
        try
        {
            var mock = new Mock<IRoomRepository>();
            TcpClient c = new TcpClient(ServerIp, ServerPort);
            var logic = new RoomLogic(mock.Object,c);
            mock.Setup(m => m.CheckExistingRoom("test", "test")).ReturnsAsync(true);
            await logic.AddRoom("test", "test","test", 25, 25);
            
            mock.Verify(m=>m.CheckExistingRoom("test","test"));
            mock.Verify(m=>m.AddRoom("test","test","test", 25 ,25));
        }
        finally
        {
            await StopServer();
        }
    }
    
    [Fact]
    public async Task AddRoom_throws_custom_error()
    {
        await StartServer();
        try
        {
            var mock = new Mock<IRoomRepository>();
            TcpClient c = new TcpClient(ServerIp, ServerPort);
            var logic = new RoomLogic(mock.Object,c);
            mock.Setup(m => m.CheckExistingRoom("test", "test"))
                .ThrowsAsync(new Exception("Room test already exists in home test"));
    
            var exception = await Assert.ThrowsAsync<Exception>(() => logic.AddRoom("test", "test", "test", 25, 25));
    
            Assert.Equal("Room test already exists in home test", exception.Message);
            mock.Verify(m => m.CheckExistingRoom("test", "test"), Times.Once);
            mock.Verify(m => m.AddRoom(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), 25, 25), Times.Never);
        }
        finally
        {
            await StopServer();
        }
    }
    
    [Fact]
    public async Task DeleteRoom_calls_for_repository()
    {
        await StartServer();
        try
        {
            var mock = new Mock<IRoomRepository>();
            TcpClient c = new TcpClient(ServerIp, ServerPort);
            var logic = new RoomLogic(mock.Object,c);
            mock.Setup(m => m.CheckNonExistingRoom("test")).ReturnsAsync(true);
        
            await logic.DeleteRoom("test");
        
            mock.Verify(m=>m.CheckNonExistingRoom("test"));
            mock.Verify(m=>m.DeleteRoom("test"));
        }
        finally
        {
            await StopServer();
        }
    }
    
    [Fact]
    public async Task DeleteRoom_throws_custom_error()
    {
        await StartServer();
        try
        {
            var mock = new Mock<IRoomRepository>();
            TcpClient c = new TcpClient(ServerIp, ServerPort);
            var logic = new RoomLogic(mock.Object,c);
            mock.Setup(m => m.CheckNonExistingRoom("test")).ThrowsAsync(new Exception("Room test doesn't exist in home"));
            var exception = await Assert.ThrowsAsync<Exception>(() => logic.DeleteRoom("test"));
        
            Assert.Equal("Room test doesn't exist in home",exception.Message);
            mock.Verify(m=>m.CheckNonExistingRoom("test"));
            mock.Verify(m=>m.DeleteRoom(It.IsAny<string>()),Times.Never);
        }
        finally
        {
            await StopServer();
        }
    }
    
    [Fact]
    public async Task EditRoom_calls_for_repository()
    {
        await StartServer();
        try
        {
            var mock = new Mock<IRoomRepository>();
            TcpClient c = new TcpClient(ServerIp, ServerPort);
            mock.Setup(m => m.CheckExistingRoom("1", "0")).ReturnsAsync(true);
            var logic = new RoomLogic(mock.Object, c);
        
            string roomId = "test";
            string roomName = "test";
            string deviceId = "1";
            int preferedTemperature = 10;
            int preferedHumidity = 10;

            await logic.EditRoom(roomId, roomName, deviceId, preferedTemperature, preferedHumidity);

            mock.Verify(m => m.EditRoom(roomId, roomName, deviceId, preferedTemperature, preferedHumidity));
        }
        finally
        {
            await StopServer();
        }
    }

    
    [Fact]
    public async Task EditRoom_throws_custom_error()
    {
        await StartServer();
        try
        {
            var mock = new Mock<IRoomRepository>();
            TcpClient c = new TcpClient(ServerIp, ServerPort);
            var logic = new RoomLogic(mock.Object,c);
            mock.Setup(m => m.EditRoom("test", null, null, 0, 0)).ThrowsAsync(new Exception("Device id can not be empty."));
    
            var exception = await Assert.ThrowsAsync<Exception>(() => logic.EditRoom("test", null, null, 0, 0));
            Assert.Equal("Device id can not be empty.", exception.Message);
    
            mock.Verify(m => m.EditRoom("test", null, null, 0, 0), Times.Never);
        }
        finally
        {
            await StopServer();
        }
    }
    
    [Fact]
    public async Task GetAllRooms_calls_for_repository()
    {
        await StartServer();
        try
        {
            var mock = new Mock<IRoomRepository>();
            TcpClient c = new TcpClient(ServerIp, ServerPort);
            var logic = new RoomLogic(mock.Object,c);
            await logic.GetAllRooms("Test");
        
            mock.Verify(m=>m.GetAllRooms("Test"));
        }
        finally
        {
            await StopServer();
        }
    }
    
    [Fact]
    public async Task GetAllRooms_throws_custom_error()
    {
        await StartServer();
        try
        {
            var mock = new Mock<IRoomRepository>();
            TcpClient c = new TcpClient(ServerIp, ServerPort);
            var logic = new RoomLogic(mock.Object,c);
            mock.Setup(m => m.GetAllRooms("test")).ThrowsAsync(new Exception("No rooms were found."));
            var exception = await Assert.ThrowsAsync<Exception>(() => logic.GetAllRooms("test"));
        
            Assert.Equal("No rooms were found.",exception.Message);
            mock.Verify(m=>m.GetAllRooms("test"));
        }
        finally
        {
            await StopServer();
        }
    }
    
    [Fact]
    public async Task GetRoomData_calls_for_repository()
    {
        await StartServer();
        try
        {
            var mock = new Mock<IRoomRepository>();
            TcpClient c = new TcpClient(ServerIp, ServerPort);
            var logic = new RoomLogic(mock.Object,c);
            await logic.GetRoomData("Test","1");
        
            mock.Verify(m=>m.GetRoomData("Test","1",false,false,false));
        }
        finally
        {
            await StopServer();
        }
    }
    [Fact]
    public async Task GetRoomData_throws_custom_error()
    {
        await StartServer();
        try
        {
            var mock = new Mock<IRoomRepository>();
            TcpClient c = new TcpClient(ServerIp, ServerPort);
            var logic = new RoomLogic(mock.Object,c);
            mock.Setup(m => m.GetRoomData("test","2",It.IsAny<bool>(),It.IsAny<bool>(),It.IsAny<bool>())).ThrowsAsync(new Exception("No room with device 2"));
            var exception = await Assert.ThrowsAsync<Exception>(() => logic.GetRoomData("test","2",true));
        
            Assert.Equal("No room with device 2",exception.Message);
            mock.Verify(m=>m.GetRoomData("test","2",true,false,false));
        }
        finally
        {
            await StopServer();
        }
    }
    [Fact]
    public async Task SetRadiatorLevel_calls_for_repo()
    {
        await StartServer();
        try
        {
            var mock = new Mock<IRoomRepository>();
            TcpClient c = new TcpClient(ServerIp, ServerPort);
            var logic = new RoomLogic(mock.Object,c);
            await logic.SetRadiatorLevel("1", 2);
            mock.Verify(m=>m.SetRadiatorLevel("1",2));
        }
        finally
        {
            await StopServer();
        }
    }
    
    [Fact]
    public async Task SetRadiatorLevel_uses_stream_to_send_data()
    {
        await StartServer();
        try
        {
            var mockRepository = new Mock<IRoomRepository>();
            TcpClient c = new TcpClient(ServerIp, ServerPort);
            var logic = new RoomLogic(mockRepository.Object, c);
            var clientField = typeof(RoomLogic).GetField("client", BindingFlags.NonPublic | BindingFlags.Instance);
            clientField.SetValue(logic, c);
            var streamField = typeof(RoomLogic).GetField("stream", BindingFlags.NonPublic | BindingFlags.Instance);
            NetworkStream s = c.GetStream();
            streamField.SetValue(logic, s);

            mockRepository.Setup(m => m.SetRadiatorLevel("1", 4)).Returns(Task.CompletedTask);
            
            await logic.SetRadiatorLevel("1", 4);

            mockRepository.Verify(repo => repo.SetRadiatorLevel("1", 4), Times.Once);
            Assert.True(logic.writeAsyncCalled, "WriteAsync was not called on the NetworkStream.");
        }
        finally
        {
            await StopServer();
        }
    }


    [Fact]
    public async Task GetRadiatorLevel_calls_for_repository()
    {
        await StartServer();
        try
        {
            var mock = new Mock<IRoomRepository>();
            TcpClient c = new TcpClient(ServerIp, ServerPort);
            var logic = new RoomLogic(mock.Object,c);
            await logic.GetRadiatorLevel("1");
            mock.Verify(m=>m.GetRadiatorLevel("1"));
        }
        finally
        {
            await StopServer();
        }
    }
    
    [Fact]
    public async Task GetRadiatorLevel_throws_error()
    {
        await StartServer();
        try
        {
            var mock = new Mock<IRoomRepository>();
            TcpClient c = new TcpClient(ServerIp, ServerPort);
            var logic = new RoomLogic(mock.Object,c);
            mock.Setup(m => m.GetRadiatorLevel("1")).ThrowsAsync(new Exception("Room  doesn't exist in home"));
            var exception = await Assert.ThrowsAsync<Exception>(() => logic.GetRadiatorLevel("1"));
        
            Assert.Equal("Room  doesn't exist in home",exception.Message);
            mock.Verify(m=>m.GetRadiatorLevel("1"));
            mock.Verify(m=>m.DeleteRoom(It.IsAny<string>()),Times.Never);
        }
        finally
        {
            await StopServer();
        }
    }

    [Fact]
    public async Task SaveWindowState_uses_stream_to_send_data_upon_existing_window_data()
    {
        await StartServer();
        try
        {
            var mockRepository = new Mock<IRoomRepository>();
            TcpClient c = new TcpClient(ServerIp, ServerPort);
            var logic = new RoomLogic(mockRepository.Object, c);
            var clientField = typeof(RoomLogic).GetField("client", BindingFlags.NonPublic | BindingFlags.Instance);
            clientField.SetValue(logic, c);
            var streamField = typeof(RoomLogic).GetField("stream", BindingFlags.NonPublic | BindingFlags.Instance);
            NetworkStream s = c.GetStream();
            streamField.SetValue(logic, s);
            mockRepository.Setup(m => m.GetWindowState("1")).ReturnsAsync(true);
            mockRepository.Setup(m => m.SaveWindowState("1", false)).Returns(Task.CompletedTask);
            
            await logic.SaveWindowState("1", false);

            mockRepository.Verify(repo => repo.SaveWindowState("1", false), Times.Once);
            Assert.True(logic.writeAsyncCalled, "WriteAsync was not called on the NetworkStream.");
        }
        finally
        {
            await StopServer();
        }
    }
    [Fact]
    public async Task SaveWindowState_uses_stream_to_send_data_upon_non_existing_window_data()
    {
        await StartServer();
        try
        {
            var mockRepository = new Mock<IRoomRepository>();
            TcpClient c = new TcpClient(ServerIp, ServerPort);
            var logic = new RoomLogic(mockRepository.Object, c);
            var clientField = typeof(RoomLogic).GetField("client", BindingFlags.NonPublic | BindingFlags.Instance);
            clientField.SetValue(logic, c);
            var streamField = typeof(RoomLogic).GetField("stream", BindingFlags.NonPublic | BindingFlags.Instance);
            NetworkStream s = c.GetStream();
            streamField.SetValue(logic, s);

            mockRepository.Setup(m => m.GetWindowState("1")).ReturnsAsync(false);
            mockRepository.Setup(m => m.SaveWindowState("1", true)).Returns(Task.CompletedTask);

            await logic.SaveWindowState("1", true);

            mockRepository.Verify(repo => repo.SaveWindowState("1", true), Times.Once);
            Assert.True(logic.writeAsyncCalled, "WriteAsync was not called on the NetworkStream.");
        }
        finally
        {
            await StopServer();
        }
    }


    [Fact]
    public async Task SaveWindowState_does_not_use_stream_to_send_data_upon_non_existing_window_data_with_false_value()
    {
        await StartServer();
        try
        {
            var mockRepository = new Mock<IRoomRepository>();
            TcpClient c = new TcpClient(ServerIp, ServerPort);
            var logic = new RoomLogic(mockRepository.Object, c);
            var clientField = typeof(RoomLogic).GetField("client", BindingFlags.NonPublic | BindingFlags.Instance);
            clientField.SetValue(logic, c);
            var streamField = typeof(RoomLogic).GetField("stream", BindingFlags.NonPublic | BindingFlags.Instance);
            var stream = c.GetStream();
            streamField.SetValue(logic, stream);
            mockRepository.Setup(m => m.GetWindowState("1")).ReturnsAsync(false);
            mockRepository.Setup(m => m.SaveWindowState("1", true)).Returns(Task.CompletedTask);

            
            await logic.SaveWindowState("1", false);

            mockRepository.Verify(repo => repo.SaveWindowState("1", false), Times.Never);
            Assert.False(_writeAsyncCalled, "WriteAsync was called on the NetworkStream.");
        }
        finally
        {
            await StopServer();
        }
    }
    [Fact]
    public async Task SaveWindowState_catches_Exception()
    {
        await StartServer();
        try
        {
            var mockRepository = new Mock<IRoomRepository>();
            TcpClient c = new TcpClient(ServerIp, ServerPort);
            var logic = new RoomLogic(mockRepository.Object, c);
            var clientField = typeof(RoomLogic).GetField("client", BindingFlags.NonPublic | BindingFlags.Instance);
            clientField.SetValue(logic, c);
            var streamField = typeof(RoomLogic).GetField("stream", BindingFlags.NonPublic | BindingFlags.Instance);
            var stream = c.GetStream();
            streamField.SetValue(logic, stream);
            mockRepository.Setup(m => m.GetWindowState("1")).ThrowsAsync(new Exception("Room does not exist."));
            _writeAsyncCalled = false;
            
            var exception = await Assert.ThrowsAsync<Exception>(() => logic.GetWindowState("1"));
        
            Assert.Equal($"Room does not exist.",exception.Message);
            mockRepository.Verify(repo => repo.SaveWindowState("1", false), Times.Never);

        }
        finally
        {
            await StopServer();
        }
    }

    [Fact]
    public async Task GetWindowState_calls_for_repo()
    {
        await StartServer();
        try
        {
            var mockRepository = new Mock<IRoomRepository>();
            TcpClient c = new TcpClient(ServerIp, ServerPort);
            var logic = new RoomLogic(mockRepository.Object, c);
            var clientField = typeof(RoomLogic).GetField("client", BindingFlags.NonPublic | BindingFlags.Instance);
            clientField.SetValue(logic, c);

            await logic.GetWindowState("1");
        
            mockRepository.Verify(repo => repo.GetWindowState("1"), Times.Once);

        }
        finally
        {
            await StopServer();
        }
    }
    
    [Fact]
    public async Task GetWindowState_throws_exception()
    {
        await StartServer();
        try
        {
            var mockRepository = new Mock<IRoomRepository>();
            TcpClient c = new TcpClient(ServerIp, ServerPort);
            var logic = new RoomLogic(mockRepository.Object, c);
            var clientField = typeof(RoomLogic).GetField("client", BindingFlags.NonPublic | BindingFlags.Instance);
            clientField.SetValue(logic, c);
            mockRepository.Setup(m => m.GetWindowState("1")).ThrowsAsync(new Exception("$Room does not exist"));
            
            var exception = await Assert.ThrowsAsync<Exception>(() => logic.GetWindowState("1"));
        
            Assert.Equal("$Room does not exist", exception.Message);
            mockRepository.Verify(repo => repo.GetWindowState("1"), Times.Once);
        }
        finally
        {
            await StopServer();
        }
    }

    [Fact]
    public async Task SetLightState_calls_for_repo()
    {
        await StartServer();
        try
        {
            var mockRepository = new Mock<IRoomRepository>();
            TcpClient c = new TcpClient(ServerIp, ServerPort);
            var logic = new RoomLogic(mockRepository.Object, c);
            var clientField = typeof(RoomLogic).GetField("client", BindingFlags.NonPublic | BindingFlags.Instance);
            clientField.SetValue(logic, c);

            await logic.SetLightState("1",2);
        
            mockRepository.Verify(repo => repo.SetLightState("1",2), Times.Once);
        }
        finally
        {
            await StopServer();
        }
    }
    
    [Fact]
    public async Task SetLightState_writes_in_stream()
    {
        await StartServer();
        try
        {
            var mockRepository = new Mock<IRoomRepository>();
            TcpClient c = new TcpClient(ServerIp, ServerPort);
            var logic = new RoomLogic(mockRepository.Object, c);
            var clientField = typeof(RoomLogic).GetField("client", BindingFlags.NonPublic | BindingFlags.Instance);
            clientField.SetValue(logic, c);
            var streamField = typeof(RoomLogic).GetField("stream", BindingFlags.NonPublic | BindingFlags.Instance);
            NetworkStream s = c.GetStream();
            streamField.SetValue(logic, s);

            mockRepository.Setup(m => m.SetLightState("1", 2)).Returns(Task.CompletedTask);

            await logic.SetLightState("1", 2);

            mockRepository.Verify(repo => repo.SetLightState("1", 2), Times.Once);
            Assert.True(logic.writeAsyncCalled, "WriteAsync hasn't been called");
        }
        finally
        {
            await StopServer();
        }
    }

    
    [Fact]
    public async Task GetLightState_calls_for_repo()
    {
        await StartServer();
        try
        {
            var mockRepository = new Mock<IRoomRepository>();
            TcpClient c = new TcpClient(ServerIp, ServerPort);
            var logic = new RoomLogic(mockRepository.Object, c);
            var clientField = typeof(RoomLogic).GetField("client", BindingFlags.NonPublic | BindingFlags.Instance);
            clientField.SetValue(logic, c);

            await logic.GetLightState("1");
        
            mockRepository.Verify(repo => repo.GetLightState("1"), Times.Once);

        }
        finally
        {
            await StopServer();
        }
    }
    
    [Fact]
    public async Task GetLightState_throws_exception()
    {
        await StartServer();
        try
        {
            var mockRepository = new Mock<IRoomRepository>();
            TcpClient c = new TcpClient(ServerIp, ServerPort);
            var logic = new RoomLogic(mockRepository.Object, c);
            var clientField = typeof(RoomLogic).GetField("client", BindingFlags.NonPublic | BindingFlags.Instance);
            clientField.SetValue(logic, c);
            mockRepository.Setup(m => m.GetLightState("1")).ThrowsAsync(new Exception($"Room  doesn't exist in home"));
            
            var exception = await Assert.ThrowsAsync<Exception>(() => logic.GetLightState("1"));
        
            Assert.Equal($"Room  doesn't exist in home", exception.Message);
            mockRepository.Verify(repo => repo.GetLightState("1"), Times.Once);
        }
        finally
        {
            await StopServer();
        }
    }
}