    using System.Diagnostics;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Xml.Serialization;
    using ECC;
    using ECC.Interface;

    public class Server
    {
        private TcpListener listener;
        private Thread serverThread;
        private bool isRunning;
        private IEncryptionService enc = new EncryptionService(Convert.FromBase64String("qKBL+IAOLbn+jLnFJEYp8KAmlAe4iVQVfa2K4d9huA4="),Convert.FromBase64String("cRooWgwV4QTvQxZkqOZRHw=="));

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
                        // Check if the message starts with "Connected:"
                        string messageHeader = Encoding.ASCII.GetString(buffer, 0, Math.Min("Connected:".Length, bytesRead));
                        if (messageHeader.StartsWith("Connected:"))
                        {
                            // Handle the connected message
                            //string publicKey = Encoding.ASCII.GetString(buffer, "Connected:".Length, bytesRead - "Connected:".Length).Trim();
                            // Generate shared secret from their PU and our PK 
                            //Encryption.DeriveSymmetricKey();
                        }
                        else if (bytesRead > 0)
                        {
                            // Decrypt the buffer directly
                            Console.WriteLine(messageHeader);
                            Console.WriteLine("messageHeader");
                            string decryptedData = enc.Decrypt(buffer.Take(bytesRead).ToArray());
                            Console.WriteLine("RETURNED");
                            Console.WriteLine(decryptedData);
                        }
                    }

                    /*  Will be used to save data received from IoT to db 
                    switch (receivedMessage)
                    {
                        case  "temperature" :
                            //database
                        default:
                            break;
                    }
                    //Communicator.Instance.Send("Force");
                    */
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error accepting client: " + e.Message);
                    isRunning = false;
                }
            }
            listener.Stop();
        }
        

        public void StopServer()
        {
            isRunning = false;
            listener.Stop();
            serverThread.Join();
            Console.WriteLine("Server stopped.");
        }
    }

