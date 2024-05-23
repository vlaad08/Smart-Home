using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading;
using ECC;
using ECC.Encryption;
using ECC.Interface;

public class Server
{
    private TcpListener listener;
    private Thread serverThread;
    private bool isRunning;
    private IEncryptionService enc = new EncryptionService("S3cor3P45Sw0rD@f"u8.ToArray(),null);
    private static readonly HttpClient httpClient = new HttpClient();
    public Server(int port)
    {
        IPAddress localAddr = IPAddress.Parse("192.168.137.14");
        listener = new TcpListener(localAddr, port);
        isRunning = true;
        listener.Start();
        serverThread = new Thread(() => ListenForClients());
        serverThread.Start();
    }
    private async void ListenForClients()
    {
        Console.WriteLine("Server started, listening for clients...");
        while (isRunning)
        {
            try
            {
                TcpClient newClient = listener.AcceptTcpClient();
                Console.WriteLine("Client connected.");
                NetworkStream stream = await Communicator.Instance.UpdateClient(newClient);

                byte[] buffer = new byte[1024];

                // Read data from the network stream
                int bytesRead;
                string receivedMessage = "";
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    // Convert the received data to a string
                    receivedMessage = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    Console.WriteLine(receivedMessage);

                    // Recognize that we are receiving their PU
                    if (receivedMessage.StartsWith("Connected:"))
                    {
                        // Generate shared secret from their PU and our PK 
                        // Encryption.GenSharedSecret(receivedMessage);
                        //Encryption.DeriveSymmetricKey();
                    }
                    else
                    {
                        string decryptedData = enc.Decrypt(buffer.Take(bytesRead).ToArray());
                        Console.WriteLine(decryptedData);
                        // Save data based on the received message
                        switch (decryptedData)
                        {
                            case var message when message.Contains("T:") && message.Contains("H:"):
                                {
                                    string[] parts = message.Split(new[] { ' ', '-', ':', 'H' }, StringSplitOptions.RemoveEmptyEntries);

                                    if (parts.Length >= 4)
                                    {
                                        string deviceId = parts[0];
                                        string tempString = parts[2];
                                        string humString = parts[3];
                                        Console.WriteLine(deviceId);
                                        Console.WriteLine(tempString);
                                        Console.WriteLine(humString);
                                        if (double.TryParse(tempString, out double tempValue) && double.TryParse(humString, out double humValue))
                                        {
                                            Console.WriteLine(deviceId);
                                            Console.WriteLine(tempValue);
                                            Console.WriteLine(humValue);
                                            SaveTemperatureAsync(deviceId, tempValue);
                                            SaveHumidityAsync(deviceId, humValue);
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
                                         SaveLightAsync(deviceId, lightValue);
                                    }
                                }
                                break;

                            default:
                                Console.WriteLine("Unrecognized message format.");
                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error accepting client: " + e.Message);
                isRunning = false;
            }
        }
        listener.Stop();
    }

    private async Task SaveTemperatureAsync(string deviceId, double value)
    {
        try
        {
            HttpResponseMessage response = await httpClient.PostAsync($"http://localhost:5084/temperature/devices/{deviceId}/{value}", null);

            Console.WriteLine(response.ToString());
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Temperature saved successfully.");
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
            HttpResponseMessage response = await httpClient.PostAsync($"http://localhost:5084/humidity/devices/{deviceId}/{value}", null);

            Console.WriteLine(response.ToString());
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Humidity saved successfully.");
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
            HttpResponseMessage response = await httpClient.PostAsync($"http://localhost:5084/light/devices/{deviceId}/{value}", null);

            Console.WriteLine(response.ToString());
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Light level saved successfully.");
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
