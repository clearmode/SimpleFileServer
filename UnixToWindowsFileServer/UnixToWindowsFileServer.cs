using System;
using System.Net;
using System.Net.Sockets;

namespace UnixToWindowsFileServer
{
    class UnixToWindowsFileServer
    {
        static int _port;

        public static void Main(string[] args)
        {
            ParseArgs(args);

            if(_port != 0)
            {
                Server fileServer = new Server(_port);
            }
            else
            {
                Server fileServer = new Server();
            }
        }


        public static void ParseArgs(string[] args)
        {
            if (args.Length > 0)
            {
                _port = Int32.Parse(args[0]);
            }
        }

    }
}
