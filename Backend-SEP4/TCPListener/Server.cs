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
        IPAddress localAddr = IPAddress.Parse("172.20.10.4");
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
                        case var message when message.StartsWith("T:"): // received temperature reading starts with T: and has for example a number like this 21.21
                            string tempString = message.Substring(2); // Extract temperature value
                            if (double.TryParse(tempString, out double tempValue))
                            {
                                Console.WriteLine(tempValue);
                                await SaveTemperatureAsync(tempValue);
                                Console.WriteLine("after in switch");
                            }
                            break;

                        default:
                            await SaveTemperatureAsync(40);
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
    
    private async Task SaveTemperatureAsync(double value)
    {
        try
        {
            string id = "1";
            string valuestring = value.ToString();
            HttpResponseMessage response = await httpClient.PostAsync($"http://localhost:5084/temperature/devices/1/20", null);
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


    public void StopServer()
    {
        isRunning = false;
        listener.Stop();
        serverThread.Join();
        Console.WriteLine("Server stopped.");
    }
}
