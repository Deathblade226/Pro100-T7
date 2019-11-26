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
    public sealed class Client : IClientServer, IClient
    {
        public Socket HostConnection { get; private set; } = new Socket(SocketType.Stream, ProtocolType.Tcp);

        public Client()
        {
            
        }

        public void Update()
        {
    
        }

        public void ReceiveData()
        {
            throw new NotImplementedException();
        }
    }
}
