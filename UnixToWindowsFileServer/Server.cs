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

        public void ParseArgs(string[] args)
        {
            foreach (string arg in args)
            {
                //todo parse commandline arguments
            }
        }

        public string Listen() 
        {
            while (true)
            {
                Console.Write("Waiting for a connection...");

                TcpClient client = tcpServer.AcceptTcpClient();
                Console.WriteLine("Connected");

               
                string fileName = "file1" + FILE_EXTENSION;
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
