﻿    using System.Diagnostics;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Xml.Serialization;


    public class Server
    {
        private TcpListener listener;
        private Thread serverThread;
        private bool isRunning;

        public Server(int port)
        {
            IPAddress localAddr = IPAddress.Parse("10.154.208.96");
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

                        if (receivedMessage.StartsWith("Connected:"))
                        {
                            //communicator methdo
                            Communicator.Instance.GenSharedSecret(receivedMessage);
                            Communicator.Instance.DeriveSymmetricKey(); //might not need this in this exact place 😥😥
                        }
                        else
                        {
                            Console.WriteLine(Communicator.Instance.DecryptMessage(
                                receivedMessage /*2nd parameter wont be needed will be replaced w/ key*/));
                        }
                        
                        // ez mi a geci? --> "Received: " + receivedMessage);
                    }

                    switch (receivedMessage)
                    {
                        case  "temperature" :
                            //database
                        default:
                            break;
                    }
                    //Communicator.Instance.Send("Force");
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

