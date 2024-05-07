using System.Net.Sockets;
using System.Numerics;
using System.Text;
using ConsoleApp1;
using ECC.NET;


public class Communicator : ICommunicator
    {
        private static Communicator _instance;
        private static readonly object _lock = new object();
        private TcpClient client;
        private NetworkStream stream;
        private Cryptography.KeyPair keyPair;

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

        public async Task<NetworkStream> UpdateClient(TcpClient newClient)
        {
            CloseCurrentClient();
            client = newClient;
            stream = newClient.GetStream();
            await handshake();
            Console.WriteLine("Communicator updated with new client.");
            return stream;
        }

        // Send message to the current client
        private void Send(string message)
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
        
        public string getTemperature()
        {
            Send("Send temperature.");
            return null;
        }

        private async Task handshake() 
        {
            Curve curve = new Curve(Curve.CurveName.secp256r1);
            BigInteger r = Numerics.GetNumberFromGroup(curve.N, curve.Length);
            Point multipliedG = Point.Multiply(r, curve.G);
            Point addedPoints = Point.Add(multipliedG, curve.G);
            keyPair = Cryptography.GetKeyPair(curve);
            Send(keyPair.PublicKey.ToString());
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