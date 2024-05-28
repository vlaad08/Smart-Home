using System.Net;
using System.Net.Sockets;
using System.Reflection;
using DBComm.Logic;
using DBComm.Repository;
using Moq;

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
      public async Task SaveTempReading_calls_for_repo()
      {
          await StartServer();
          try
          {
              var mock = new Mock<ITemperatureRepository>();
              TcpClient c = new TcpClient(ServerIp, ServerPort);
              var logic = new TemperatureLogic(mock.Object, c);

              await logic.SaveTempReading("1", 2);
              
              mock.Verify(m=>m.SaveTemperatureReading("1",2,It.IsAny<DateTime>()));
          }
          finally
          {
              await StopServer();
          }
      }
      
      
}