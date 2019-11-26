using System;
using System.Net;
using System.Net.Sockets;

namespace NetworkingTestsHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            EndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5555);
            Socket s = new Socket(SocketType.Stream, ProtocolType.Tcp);
            s.Connect(ep);
        }
    }
}
