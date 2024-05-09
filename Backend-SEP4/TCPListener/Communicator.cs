using System;
using System.Globalization;
using System.Net.Sockets;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using ConsoleApp1;
using ECC.NET;
using ECC.Encryption;

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

    public async Task<NetworkStream> UpdateClient(TcpClient newClient)
    {
        CloseCurrentClient();
        client = newClient;
        stream = newClient.GetStream();
        await handshake();
        Console.WriteLine("Communicator updated with new client.");
        return stream;
    }

    // Send our PK to IoT
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
    
    // Send a string to current client
    private void Send(string message)
    {
        // Encrypt message before sending it
        string encMsg = Encryption.EncryptMessage(message);
        
        // Convert to byte[] before sending it
        byte[] data = Encoding.UTF8.GetBytes(encMsg);
        
        Send(data);
    }

    public Task<string> getTemperature()
    {
        Send("Send temperature.");
        return null;
    }

    public Task SwitchWindow()
    {
        Send("Switch window");
        Console.WriteLine("Switch window");
        return Task.CompletedTask;
    }

    //Sending PU to IoT
    private async Task handshake()
    {
        Curve curve = new Curve(Curve.CurveName.secp256r1);
        BigInteger r = Numerics.GetNumberFromGroup(curve.N, curve.Length);
        Point multipliedG = Point.Multiply(r, curve.G);
        Point addedPoints = Point.Add(multipliedG, curve.G);
        //keyPair = Cryptography.GetKeyPair(curve);
        
        Encryption.SaveKeyPair(Cryptography.GetKeyPair(curve));
        
        //Converting hex PK to uint8_[64] for IoT and sending it
        string hexKey = Encryption.GetKeyPair().PublicKey.ToString();
        Send(Encryption.HexStringToByteArray(hexKey));
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
