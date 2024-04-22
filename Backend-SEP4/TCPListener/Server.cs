using System.Net;
using System.Net.Sockets;
using System.Xml.Serialization;


public class Server
{
    private TcpListener listener;
    private Thread serverThread;
    private bool isRunning;

    public Server(int port)
    {
        IPAddress localAddr = IPAddress.Parse("192.168.180.220");
        listener = new TcpListener(localAddr, port);
        isRunning = true;
        listener.Start();
        serverThread = new Thread(() => ListenForClients());
        serverThread.Start();
    }

    private void ListenForClients()
    {
        Console.WriteLine("Server started, listening for clients...");
        while (isRunning)
        {
            try
            {
                TcpClient newClient = listener.AcceptTcpClient();
                Console.WriteLine("Client connected.");
                Communicator.Instance.UpdateClient(newClient);
                //Communicator.Instance.Send("Force");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error accepting client: " + e.Message);
                isRunning = false;
            }
        }
        listener.Stop();
    }

    public void StopServer()
    {
        isRunning = false;
        listener.Stop();
        serverThread.Join();
        Console.WriteLine("Server stopped.");
    }
}

