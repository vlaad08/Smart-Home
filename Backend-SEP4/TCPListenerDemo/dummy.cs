using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class IoTCommunicationHandler
{
    private readonly string ipAddress;
    private readonly int port;
    private Socket socket;
    private string messageToSend;
    private readonly object messageLock = new object();

    public IoTCommunicationHandler(string ipAddress, int port)
    {
        this.ipAddress = ipAddress;
        this.port = port;
    }

    public void StartCommunication()
    {
        try
        {
            // Create a socket and connect to the IoT device
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(new IPEndPoint(IPAddress.Parse(ipAddress), port));

            // Start a separate thread to handle communication
            Thread communicationThread = new Thread(CommunicationLoop);
            communicationThread.IsBackground = true;
            communicationThread.Start();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error connecting to IoT device: {ex.Message}");
        }
    }

    public void SendMessage(string message)
    {
        lock (messageLock)
        {
            // Set the message to be sent
            messageToSend = message;
        }
    }

    private void CommunicationLoop()
    {
        try
        {
            while (true)
            {
                // Check if there's a message to send
                string message = null;
                lock (messageLock)
                {
                    if (!string.IsNullOrEmpty(messageToSend))
                    {
                        message = messageToSend;
                        messageToSend = null; // Clear the message after sending
                    }
                }

                // Send the message if available
                if (!string.IsNullOrEmpty(message))
                {
                    byte[] data = Encoding.ASCII.GetBytes(message);
                    socket.Send(data);
                    Console.WriteLine($"Message sent to IoT device: {message}");
                }

                // Other communication handling logic can go here

                // Simulate some delay before the next iteration
                Thread.Sleep(1000);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in communication loop: {ex.Message}");
        }
        finally
        {
            // Close the socket when done
            socket.Close();
        }
    }
}
