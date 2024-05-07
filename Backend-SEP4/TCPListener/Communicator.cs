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


        // Send a byte array to the current client
        private void Send(byte[] data)
        {
            if (client != null && stream != null)
            {
                try
                {
                    stream.Write(data, 0, data.Length);
                    Console.WriteLine("Data sent successfully.");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error sending data: " + e.Message);
                    CloseCurrentClient();
                }
            }
            else
            {
                Console.WriteLine("No client connected to send data.");
            }
        }
        
        private void Send(string message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            Send(data);
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
            
            //Converting hex PK to uint8_[64] for IoT
            string hexKey = keyPair.PublicKey.ToString();
            Send(HexStringToByteArray(hexKey));
        }

        
        //kex conversions
        private byte[] HexStringToByteArray(string hex)
        {
            //Although we can assume that the key is valid with an even lenght we do exception handling
            if (string.IsNullOrEmpty(hex))
                throw new ArgumentException("Hex string is null or empty.");
            if (hex.Length % 2 != 0)
                throw new ArgumentException("Hex string must have an even length.");

            byte[] bytes = new byte[hex.Length / 2];
            for (int i = 0; i < hex.Length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
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