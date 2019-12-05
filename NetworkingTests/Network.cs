using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NetworkingTests
{
    public class Network
    {
        public static void Main(string[] args)
        {
            IPEndPoint local = new IPEndPoint(IPAddress.Any, 5555);
            Socket listener = new Socket(local.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(local);
            listener.Listen(1);
            
            Socket accepted = listener.Accept();

            Tests(accepted);
        }

        public static async void Tests(Socket accepted)
        {
            while (true)
            {
                byte[] words = new byte[256];
                accepted.BeginReceive(words, 0, 0, SocketFlags.None, null, null);

                string s = Encoding.ASCII.GetString(words);
                Console.WriteLine(s);
            }
        }
    }
}
