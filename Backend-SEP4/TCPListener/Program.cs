using System.Net;
using System.Net.Sockets;
using System.Text;

int port = 8000;

//Change IP addres to local network X localhost
IPAddress localAddr = IPAddress.Parse("10.154.216.96");
TcpListener server = new TcpListener(localAddr, port);

byte[] bytes = new byte[256];
string data;
server.Start();



try
{
    // Enter the listening loop
    while (true)
    {
        Console.WriteLine("Waiting for a connection...");
        TcpClient client = server.AcceptTcpClient();
        Console.WriteLine("Connected!");

        data = null;

        // Get a stream object for reading and writing
        NetworkStream stream = client.GetStream();

        int i;

        // Loop to receive all the data sent by the client
        while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
        {
            // Translate data bytes to a ASCII string
            data = Encoding.ASCII.GetString(bytes, 0, i);
            Console.WriteLine("Received: {0}", data);
        }

        // Shutdown and end connection
        client.Close();
    }
}
catch (SocketException e)
{
    Console.WriteLine("SocketException: {0}", e);
}
finally
{
    // Stop listening for new clients
    server.Stop();
}