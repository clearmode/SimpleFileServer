using System;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace UnixToWindowsFileServer
{
    class Server
    {
        const int DEFAULT_PORT = 804;
        const string FILE_EXTENSION = ".dat";

        TcpListener tcpServer;
        byte[] bytes;

        public Server(int port = DEFAULT_PORT)
        {
            try
            {
                bytes = new byte[100];
                IPAddress serverAddr = IPAddress.Any;
                var serverEndPoint = new IPEndPoint(serverAddr, port);
                tcpServer = new TcpListener(serverEndPoint);
                tcpServer.Start();
                Console.WriteLine("Listening on port " + port);
                Listen();
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                tcpServer.Stop();
            }
        }

        private void Listen() 
        {
            while (true)
            {
                Console.Write("Waiting for a connection...");

                TcpClient client = tcpServer.AcceptTcpClient();
                Console.WriteLine("Connected");

               
                string fileName = DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + FILE_EXTENSION;
                BinaryWriter writer = new BinaryWriter(File.Open(fileName, FileMode.OpenOrCreate));

                NetworkStream stream = client.GetStream();

                int i;

                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    for(int j = 0; j<i;j++)
                    {
                        writer.Write(bytes[j]);
                    }
                }

                Console.WriteLine("Closing");
                writer.Close();
                client.Close();
            }
        }

    }
}
