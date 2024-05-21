using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using ECC.Encryption;

public class Server
{
    private TcpListener listener;
    private Thread serverThread;
    private bool isRunning;
    private static readonly HttpClient httpClient = new HttpClient();

    public Server(int port)
    {
        IPAddress localAddr = IPAddress.Parse("192.168.0.220");
        listener = new TcpListener(localAddr, port);
        isRunning = true;
        listener.Start();
        serverThread = new Thread(() => ListenForClients());
        serverThread.Start();
        //SaveTemperatureAsync("1", 20);
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
                    Console.WriteLine("segg");
                    // Convert the received data to a string
                    receivedMessage = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    Console.WriteLine(receivedMessage);
                    
                    // Recognize that we are receiving their PU
                    if (receivedMessage.StartsWith("Connected:"))
                    {
                        // Generate shared secret from their PU and our PK 
                        Encryption.GenSharedSecret(receivedMessage);
                        //Encryption.DeriveSymmetricKey();
                    }
                    else
                    {
                        // Print the decrypted received message
                        //Console.WriteLine(Encryption.DecryptMessage(receivedMessage));
                    }

                    // Save data based on the received message
                    switch (receivedMessage)
                    {
                        case var message when message.Contains("T:") && message.Contains("H:"):
                        {
                            string[] parts = message.Split(new[] { ' ', '-' }, StringSplitOptions.RemoveEmptyEntries);
                            if (parts.Length >= 5)
                            {
                                string deviceId = parts[0];
                                string tempString = parts[2];
                                string humString = parts[4];
                
                                if (double.TryParse(tempString, out double tempValue) && double.TryParse(humString, out double humValue))
                                {
                                    await SaveTemperatureAsync(deviceId, tempValue);
                                    await SaveHumidityAsync(deviceId, humValue);
                                }
                            }
                        }
                            break;

                        case var message when message.StartsWith("L:"):
                        
                        // Convert the received data to a string
                        receivedMessage = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                        
                        // Recognize that we are receiving their PU
                        if (receivedMessage.StartsWith("Connected:"))
                        {
                            string publicKey = receivedMessage.Substring("Connected:".Length).Trim();
                            // Generate shared secret from their PU and our PK 
                            //Encryption.DeriveSymmetricKey();
                        }
                        else if(receivedMessage!= null)

                        {
                            string[] parts = message.Substring(2).Split(' ');
                            if (parts.Length > 1 && double.TryParse(parts[1], out double lightValue))
                            {
                                string deviceId = parts[0];
                                await SaveLightAsync(deviceId, lightValue);
                            }
                        }
                            break;

                        default:
                            Console.WriteLine("Unrecognized message format.");
                            break;
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
            string id = deviceId;
            string valuestring = value.ToString();
            HttpResponseMessage response = await httpClient.PostAsync($"http://localhost:5084/temperature/devices/1/{value}", null);
            //HttpResponseMessage response = await httpClient.GetAsync($"http://localhost:5084/temperature/1");

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
            string id = deviceId;
            string valuestring = value.ToString();
            HttpResponseMessage response = await httpClient.PostAsync($"http://localhost:5084/humidity/devices/1/{value}", null);

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
    
    private async Task SaveLightAsync(string deviceId, double value)
    {
        try
        {
            string id = deviceId;
            string valuestring = value.ToString();
            HttpResponseMessage response = await httpClient.PostAsync($"http://localhost:5084/light/devices/1/{value}", null);

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


    public void StopServer()
    {
        isRunning = false;
        listener.Stop();
        serverThread.Join();
        Console.WriteLine("Server stopped.");
    }
}
