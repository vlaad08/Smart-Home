using System;
using System.Globalization;
using System.Net.Sockets;
using System.Numerics;
using System.Security.Cryptography;
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
    private Point sharedSecret;
    private byte[] symmetricKey;

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

    // Send a byte array to the current client this for key sending this shi doesnt need to be encrypted n fucked with
    private void Send(byte[] data)
    {
        try
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
        catch(Exception)
        {
            throw new ArgumentNullException("TCP listener on IOT part is not running");
        }
    }
    //send a string to current client
    private void Send(string message)
    {
        //string encMsg = EncryptMessage(message, /*second parameter wont be needed if i bring myself to change the keys*/);
        string encMsg = EncryptMessage(message);
        byte[] data = Encoding.UTF8.GetBytes(encMsg);
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
    //
    public byte[] HexStringToByteArray(string hex)
    {
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

    //get the pu from iot convert it to point and generate the secred w/ ECC form PK PU and the secp256 curve
    public void GenSharedSecret(string message)
    {
        string ioTPublicKeyHex = message.Substring("Connected:".Length).Trim();
        try
        {
            Curve curve = new Curve(Curve.CurveName.secp256r1);
            Point ioTPublicKeyPoint = HexStringToPoint(ioTPublicKeyHex, curve);
            sharedSecret = Cryptography.GetSharedSecret(keyPair.PrivateKey, ioTPublicKeyPoint);
            Console.WriteLine($"Shared secret computed: ({sharedSecret.X}, {sharedSecret.Y})");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error generating shared secret: {e.Message}");
        }
    }
    //to make point from iot public key so shared secret can be generated
    private Point HexStringToPoint(string hex, Curve curve)
    {
        if (hex.Length % 2 != 0 || hex.Length < 2 * ((curve.Length + 7) / 8))
            throw new ArgumentException("Hex string does not represent a valid point on the curve.");

        string xHex = hex.Substring(2, hex.Length / 2 - 1); // TODO check: does the hex string start with 04????
        string yHex = hex.Substring(2 + hex.Length / 2 - 1);

        BigInteger x = BigInteger.Parse(xHex, NumberStyles.AllowHexSpecifier);
        BigInteger y = BigInteger.Parse(yHex, NumberStyles.AllowHexSpecifier);
        
        Point publicKey = new Point(x, y, curve);
        curve.CheckPoint(publicKey, new ArgumentException("The public key point is not on the curve."));
        return publicKey;
    }

    //the following is basically chatgpt but i give up, gl

    //getting the symmetric key from the shared secret by hashing the X coord (i think this is the BIGINT the privy key
    public void DeriveSymmetricKey()
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] key = sha256.ComputeHash(sharedSecret.X.ToByteArray());
            symmetricKey = key;
        }
    }

    //encrypt
    //actually this shit ass fuck shit needs to return an encrypted byte[] ??? 🤔🤔🤔
    //actually it can return string but no its shit with the send cuz we send strings as byte[]s anyways no? 🤔
    public string EncryptMessage(string plainText/*, byte[] key*/) //replace the key with actual key symmetricKey
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = symmetricKey;
            aesAlg.GenerateIV();

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (var msEncrypt = new MemoryStream())
            {
                msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                using (var swEncrypt = new StreamWriter(csEncrypt))
                {
                    swEncrypt.Write(plainText);
                }
                return Convert.ToBase64String(msEncrypt.ToArray());
            }
        }
    }
    //decrypt
    public string DecryptMessage(string cipherText/*, byte[] key = symmetricKey*/) //replace the key with actual key symmetricKey
    {
        byte[] fullCipher = Convert.FromBase64String(cipherText);

        using (Aes aesAlg = Aes.Create())
        {
            byte[] iv = new byte[aesAlg.BlockSize / 8];
            byte[] cipher = new byte[fullCipher.Length - iv.Length];

            Array.Copy(fullCipher, iv, iv.Length);
            Array.Copy(fullCipher, iv.Length, cipher, 0, cipher.Length);

            aesAlg.Key = symmetricKey;
            aesAlg.IV = iv;

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (var msDecrypt = new MemoryStream(cipher))
            using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
            using (var srDecrypt = new StreamReader(csDecrypt))
            {
                return srDecrypt.ReadToEnd();
            }
        }
    }
    
    //mybe need later
    public Point getSharedSecret()
    {
        return sharedSecret;
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

    public void setTemperature(string temperature)
    {
        string message = $"Set temperature: {temperature}";
        Send(message);
    }
}
