    using System.Diagnostics;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Xml.Serialization;
    using ECC.Encryption;

    public class Server
    {
        private TcpListener listener;
        private Thread serverThread;
        private bool isRunning;

        public Server(int port)
        {
            IPAddress localAddr = IPAddress.Parse("192.168.137.245");
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
                        
                        // Recognize that we are receiving their PU
                        if (receivedMessage.StartsWith("Connected:"))
                        {
                            string publicKey = receivedMessage.Substring("Connected:".Length).Trim();
                            // Generate shared secret from their PU and our PK 
                            //Encryption.DeriveSymmetricKey();
                        }
                        else if(receivedMessage!= null)
                        {
                            // Print the decrypted received message
                            Console.WriteLine(Encryption.DecryptMessage(receivedMessage));
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

