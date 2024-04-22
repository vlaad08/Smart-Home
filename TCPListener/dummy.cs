using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class ArduinoListener
{
    private IPAddress localAddr;
    private int port;
    private Socket listener;

    public ArduinoListener(string ipAddress, int port)
    {
        this.localAddr = IPAddress.Parse(ipAddress);
        this.port = port;
    }

    public async Task StartListening()
    {
        // Create a TCP/IP socket
        listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            // Bind the socket to the local endpoint and listen for incoming connections
            listener.Bind(new IPEndPoint(localAddr, port));
            listener.Listen(10);

            Console.WriteLine("Waiting for a connection...");

            while (true)
            {
                // Accept connections asynchronously
                Socket handler = await listener.AcceptAsync();

                // Start a new task to handle the connection
                _ = HandleClientAsync(handler);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    public async Task SendMessageAsync(string message)
    {
        byte[] msg = Encoding.ASCII.GetBytes(message);

        try
        {
            // Send message to all connected clients
            foreach (Socket client in listener.GetClients())
            {
                await client.SendAsync(new ArraySegment<byte>(msg), SocketFlags.None);
                Console.WriteLine("Sent to client: {0}", message);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    private async Task HandleClientAsync(Socket handler)
    {
        byte[] bytes = new byte[256];
        string data = null;

        try
        {
            Console.WriteLine("Connected!");

            // Receive data from the client
            while (true)
            {
                int bytesRec = await handler.ReceiveAsync(new ArraySegment<byte>(bytes), SocketFlags.None);

                if (bytesRec == 0)
                    break;

                data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                Console.WriteLine("Received: {0}", data);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
        finally
        {
            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
        }
    }
}

static class SocketExtensions
{
    public static IEnumerable<Socket> GetClients(this Socket listener)
    {
        List<Socket> clients = new List<Socket>();

        foreach (Socket client in listener.SelectReady(new List<Socket>(), new List<Socket>(), 0))
        {
            clients.Add(client);
        }

        return clients;
    }
}

class Program
{
    static async Task Main(string[] args)
    {
        string ipAddress = "10.154.216.96";
        int port = 8000;

        ArduinoListener arduinoListener = new ArduinoListener(ipAddress, port);
        await arduinoListener.StartListening();

        // Example of sending a message to the connected clients
        await arduinoListener.SendMessageAsync("Hello from the backend!");
    }
}
