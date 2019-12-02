using Pro100_T7.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Windows.System.Threading;

namespace Pro100_T7.Models
{
    public sealed class Server : IServer
    {
        public Socket CurrentAddress { get; set; } = new Socket(SocketType.Stream, ProtocolType.Tcp);
        public Stack<Socket> Clients { get; private set; } = new Stack<Socket>();

        public Server()
        {
            
        }

        public bool IsHosting()
        {
            return Clients.Count() > 0;
        }

        public void SendData(byte[] data)
        {
            foreach (Socket s in Clients)
            {
                s.Send(data);
            }
        }

        public bool TryConnectClient(Socket client)
        {
            Socket newClient = CurrentAddress.Accept();
            if (newClient != null)
            {
                Clients.Push(newClient);
                return true;
            }

            return false;
        }
    }
}
