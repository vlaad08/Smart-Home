using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using ConsoleApp1;
using DBComm.Logic;
using DBComm.Logic.Interfaces;
using DBComm.Repository;
using Moq;

namespace Tests.TemperatureTests;

public class LightLogicTest
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
//   
   [Fact]
   public async void GetLight_calls_for_repo()
   {
       await StartServer();
       try
       {
           var mock = new Mock<ILigthRepository>();
           TcpClient c = new TcpClient(ServerIp, ServerPort);
           var logic = new LightLogic(mock.Object, c);
           await logic.GetLatestLight("1");
           mock.Verify(d => d.GetLatestLight("1"));
       }
       finally
       {
           await StopServer();
       }
   }

   [Fact]
   public async Task GetLightHistory_calls_for_repository()
   {
       await StartServer();
       try
       {
           var mock = new Mock<ILigthRepository>();
           TcpClient c = new TcpClient(ServerIp, ServerPort);
           var logic = new LightLogic(mock.Object, c);
           var now1 = DateTime.Now;
           var now2 = DateTime.Now;

           await logic.GetLightHistory("1", now1, now2);

           mock.Verify(d => d.GetHistory("1", now1, now2));
       }
       finally
       {
           await StopServer();
       }
   }
   
   [Fact]
   public async Task SetLight_calls_for_communicator()
   {
       await StartServer();
       try
       {
           var mock = new Mock<ILigthRepository>();
           TcpClient c = new TcpClient(ServerIp, ServerPort);
           var logic = new LightLogic(mock.Object,c);
           var clientField = typeof(LightLogic).GetField("client", BindingFlags.NonPublic | BindingFlags.Instance);
           clientField.SetValue(logic, c);
           var streamField = typeof(LightLogic).GetField("stream", BindingFlags.NonPublic | BindingFlags.Instance);
           NetworkStream s = c.GetStream();
           streamField.SetValue(logic, s);

           await logic.SetLight("1", 2);
           
           Assert.True(logic.writeAsyncCalled,"Write on stream hasn't been called");
       }
       finally
       {
           await StopServer();
       }
   }

   [Fact]
   public async Task SaveLightReading_calls_for_repository()
   {
       await StartServer();
       try
       {
           var mock = new Mock<ILigthRepository>();
           TcpClient c = new TcpClient(ServerIp, ServerPort);
           var logic = new LightLogic(mock.Object, c);
        
           double value = 3;
           double scaleValue = (1000 - value) / 10;
           await logic.SaveLightReading("1", value);
        
           mock.Verify(m => m.SaveLightReading("1", scaleValue, It.IsAny<DateTime>()), Times.Once);
       }
       finally
       {
           await StopServer();
       }
   }

//   
}