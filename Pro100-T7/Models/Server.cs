using Pro100_T7.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Pro100_T7.Models
{
    public sealed class Server : IClientServer, IServer
    {
        public Stack<Socket> Clients { get; private set; } = new Stack<Socket>();
        public IPAddress LocalAddress { get; set; }

        public Server()
        {
            
        }

        public void Update()
        {
            throw new NotImplementedException();
        }

        public void SendData()
        {
            foreach (Socket s in Clients)
            {
                
            }

            throw new NotImplementedException();
        }

    }
}
