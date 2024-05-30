using System.Net;
using System.Net.Sockets;
using System.Text;
using ECC;
using ECC.Interface;

public class Server
{
    private TcpListener listener;
    private Thread serverThread;
    private bool isRunning;
    private IEncryptionService enc = new EncryptionService("S3cor3P45Sw0rD@f"u8.ToArray(), null);
    private static readonly HttpClient httpClient = new HttpClient();
    private NetworkStream stream;



    /* A dictionary to store all connected clients and a boolean value to check if they are logic clients or not.
     * The key is the TcpClient object and the value is a boolean value that is true if the client is a logic client and false otherwise.
    */
    private IDictionary<TcpClient, bool> allClients = new Dictionary<TcpClient, bool>();

    private string SERVER_ADDRESS;
    private string WEB_API_ADDRESS;

    public Server(int port)
    {
        DotNetEnv.Env.TraversePath().Load();

        SERVER_ADDRESS = Environment.GetEnvironmentVariable("SERVER_ADDRESS");
        WEB_API_ADDRESS = Environment.GetEnvironmentVariable("WEB_API_ADDRESS") ?? "localhost";
        if (SERVER_ADDRESS == null)
        {
            DotNetEnv.Env.Load();
        }

        SERVER_ADDRESS = Environment.GetEnvironmentVariable("SERVER_ADDRESS") ?? "127.0.0.1";
        WEB_API_ADDRESS = Environment.GetEnvironmentVariable("WEB_API_ADDRESS") ?? "localhost";

        IPAddress localAddr = IPAddress.Parse(SERVER_ADDRESS);
        Console.WriteLine("Server address: " + localAddr.ToString());
        listener = new TcpListener(localAddr, port);
        isRunning = true;
        listener.Start();
        ListenForClients();
    }

    private void ListenForClients()
    {
        Console.WriteLine("Server started, listening for clients...");
        while (isRunning)
        {
            try
            {
                TcpClient newClient = listener.AcceptTcpClient();

                allClients.Add(newClient, false);
                Console.WriteLine("Client connected.");
                Task.Run(() => HandleClient(newClient));
            }
            catch (Exception e)
            {
                Console.WriteLine("Error accepting client: " + e.Message);
                isRunning = false;
            }
        }
        listener.Stop();
    }

    private async Task HandleClient(TcpClient client)
    {
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];

        // Read data from the network stream
        int bytesRead;
        string receivedMessage = "";
        while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
        {
            receivedMessage = Encoding.ASCII.GetString(buffer, 0, bytesRead);



            string decryptedData = enc.Decrypt(buffer.Take(bytesRead).ToArray());

            // Check if the client is a logic client and mark it as such
            if (decryptedData.StartsWith("LOGIC CONNECTED:"))
            {
                Console.WriteLine("Logic client connected.");
                allClients[client] = true;
            }
            // Save data based on the received message
            switch (decryptedData)
            {
                case var message when message.StartsWith("LOGIC:"):
                    {

                        string[] parts = message.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        string newMessage = string.Join(" ", parts.Skip(1));
                        foreach (var logicClient in allClients)
                        {
                            if (!logicClient.Value)
                            {
                                int blockSize = 16;
                                int extraBytes = newMessage.Length % blockSize;
                                if (extraBytes != 0)
                                {
                                    newMessage = newMessage.PadRight(newMessage.Length + blockSize - extraBytes, ' ');
                                }
                                byte[] messageBytes = enc.Encrypt(newMessage);
                                Send(messageBytes, logicClient.Key);
                            }
                        }
                    }
                    break;

                case var message when message.Contains("T:") && message.Contains("H:"):
                    {
                        string[] parts = message.Split(new[] { ' ', '-', ':', 'H' }, StringSplitOptions.RemoveEmptyEntries);

                        if (parts.Length >= 4)
                        {
                            string deviceId = parts[0];
                            string tempString = parts[2];
                            string humString = parts[3];

                            if (double.TryParse(tempString, out double tempValue) && double.TryParse(humString, out double humValue))
                            {
                                await SaveTemperatureAsync(deviceId, tempValue);
                                await SaveHumidityAsync(deviceId, humValue);
                            }
                        }
                    }
                    break;

                case var message when message.Contains("LIGHT:"):
                    {
                        string[] parts = message.Substring(2).Split(new[] { ' ', ':' }, StringSplitOptions.RemoveEmptyEntries);

                    if (parts.Length > 1 && double.TryParse(parts[1], out double lightValue))
                    {
                        string deviceId = message.Substring(0, 1);
                        await SaveLightAsync(deviceId, lightValue);
                    }
                } 
                break;

                case var message when message.Contains("HEllO"):
                {
                    string deviceId = message.Substring(0, 1);
                    await SendBurglarNotification(deviceId);
                }
                    
                break;

                default:
                    break;
                    {
                        Console.WriteLine("switchcase default");
                        // The first 4 characters will be used by the receiving party
                        string identifier = decryptedData.Substring(0, 4);
                        Console.WriteLine("switchcase default 1");

                        string remainingMessage = decryptedData.Substring(4);
                        Console.WriteLine("switchcase default 2");

                        foreach (var logicClient in allClients)
                        {
                            Console.WriteLine("switchcase default 3");

                            if (!logicClient.Value)
                            {
                                Console.WriteLine("switchcase default4");

                                int blockSize = 16;
                                int extraBytes = remainingMessage.Length % blockSize;
                                if (extraBytes != 0)
                                {
                                    remainingMessage = remainingMessage.PadRight(remainingMessage.Length + blockSize - extraBytes, ' ');
                                }
                                byte[] messageBytes = enc.Encrypt(identifier + remainingMessage);
                                Console.WriteLine("switchcase default 5");

                                Send(messageBytes, logicClient.Key);
                                Console.WriteLine("switchcase default 6");

                            }
                        }
                    }
                    break;
                    Console.WriteLine("Unrecognized message format.");
                    //break;
            }
        }
    }
    public static void Send(byte[] data, TcpClient receiverClient)
    {
        if (data.Length == 0)
        {
            Console.WriteLine("No data to send.");
            return;
        }

        try
        {
            NetworkStream stream = receiverClient.GetStream();
            stream.Write(data, 0, data.Length);
            Console.WriteLine("Data sent successfully.");
        }
        catch (Exception e)
        {
            Console.WriteLine("Error sending data: " + e.Message);
        }
    }

    private async Task SaveTemperatureAsync(string deviceId, double value)
    {
        try
        {
            HttpResponseMessage response = await httpClient.PostAsync($"http://{WEB_API_ADDRESS}:80/temperature/devices/{deviceId}/{value}", null);

            // Console.WriteLine(response.ToString());
            if (response.IsSuccessStatusCode)
            {
                // Console.WriteLine("Temperature saved successfully.");
            }
            else
            {
                Console.WriteLine("Failed to save temperature.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception occurred while saving temperature: {ex.Message}");
        }
    }

    private async Task SaveHumidityAsync(string deviceId, double value)
    {
        try
        {
            HttpResponseMessage response = await httpClient.PostAsync($"http://{WEB_API_ADDRESS}:80/humidity/devices/{deviceId}/{value}", null);

            // Console.WriteLine(response.ToString());
            if (response.IsSuccessStatusCode)
            {
                // Console.WriteLine("Humidity saved successfully.");
            }
            else
            {
                Console.WriteLine("Failed to save humidity.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception occurred while saving humidity: {ex.Message}");
        }
    }

    private async Task SaveLightAsync(string deviceId, double value)
    {
        try
        {
            HttpResponseMessage response = await httpClient.PostAsync($"http://{WEB_API_ADDRESS}:80/light/devices/{deviceId}/{value}", null);

            // Console.WriteLine(response.ToString());
            if (response.IsSuccessStatusCode)
            {
                // Console.WriteLine("Light level saved successfully.");
            }
            else
            {
                Console.WriteLine("Failed to save light level.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception occurred while saving light level: {ex.Message}");
        }
    }

    private async Task SendBurglarNotification(string deviceId)
    {
        try
        {
            HttpResponseMessage response = await httpClient.PostAsync($"http://localhost:5084/notifications/burglar/{deviceId}", null);

            if (response.IsSuccessStatusCode)
            {
            }
            else
            {
                Console.WriteLine("Failed to save light level.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception occurred while saving light level: {ex.Message}");
        }
    }

    public void StopServer()
    {
        isRunning = false;
        listener.Stop();
        serverThread.Join();
        Console.WriteLine("Server stopped.");
    }
}
