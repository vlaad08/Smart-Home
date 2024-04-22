using System.Net.Sockets;
using System.Text;
using ConsoleApp1;


public class Communicator : ICommunicator
    {
        private static Communicator _instance;
        private static readonly object _lock = new object();
        private TcpClient client;
        private NetworkStream stream;

        public static Communicator Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new Communicator();
                    }
                }
                return _instance;
            }
        }

        private Communicator()
        {
        }

        public void UpdateClient(TcpClient newClient)
        {
            CloseCurrentClient();
            client = newClient;
            stream = newClient.GetStream();
            Console.WriteLine("Communicator updated with new client.");
        }

        // Send message to the current client
        public void Send(string message)
        {
            if (client != null && stream != null)
            {
                try
                {
                    byte[] data = Encoding.UTF8.GetBytes(message);
                    stream.Write(data, 0, data.Length);
                    Console.WriteLine("Message sent: " + message);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error sending message: " + e.Message);
                    CloseCurrentClient();
                }
            }
            else
            {
                Console.WriteLine("No client connected to send a message.");
            }
        }

        // Close the current client connection
        private void CloseCurrentClient()
        {
            if (stream != null)
            {
                stream.Close();
                stream = null;
            }
            if (client != null)
            {
                client.Close();
                client = null;
            }
            Console.WriteLine("Client connection closed.");
        }
    }