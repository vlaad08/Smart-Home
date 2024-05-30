using System;
using System.Globalization;
using System.Net.Sockets;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using ConsoleApp1;
using ECC;
using ECC.Interface;

public class Communicator : ICommunicator
{
    private static Communicator _instance;
    private static readonly object _lock = new object();
    private TcpClient client;
    private NetworkStream stream;
    private static IEncryptionService enc;

    public static Communicator Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new Communicator();
                    enc = new EncryptionService("S3cor3P45Sw0rD@f"u8.ToArray(),null);
                }
            }
            return _instance;
        }
    }

    protected Communicator()
    {
    }

    public async Task<NetworkStream> UpdateClient(TcpClient newClient)
    {
        CloseCurrentClient();
        client = newClient;
        stream = newClient.GetStream();
        //await handshake();
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
        byte[] data = enc.Encrypt(message);
        
        Send(data);
    }

    public async Task<double> getTemperature()
    {
        Send("Send temperature.");
        return 32;
    }


    public void setTemperature(string hardwareId, int level)
    {
        Send($"Set temperature {level} on {hardwareId}");
    }

    public void setLight(string hardwareId, int level)
    {
        Send($"Set light {level} on {hardwareId}");
    }
    

    public Task SwitchWindow()
    {
        Send("Switch window");
        Console.WriteLine("Switch window");
        return Task.CompletedTask;
    }

    public Task SwitchDoor()
    {
        Send($"Switch door");
        Console.WriteLine("Switch door");
        return Task.CompletedTask;
    }


    //Sending PU to IoT
    // private async Task handshake()
    // {
    //     Curve curve = new Curve(Curve.CurveName.secp256r1);
    //     BigInteger r = Numerics.GetNumberFromGroup(curve.N, curve.Length);
    //     Point multipliedG = Point.Multiply(r, curve.G);
    //     Point addedPoints = Point.Add(multipliedG, curve.G);
    //     //keyPair = Cryptography.GetKeyPair(curve);
    //     
    //     Encryption.SaveKeyPair(Cryptography.GetKeyPair(curve));
    //     
    //     //Converting hex PK to uint8_[64] for IoT and sending it
    //     string hexKey = Encryption.GetKeyPair().PublicKey.ToString();
    //     Send(Encryption.HexStringToByteArray(hexKey));
    // }

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
