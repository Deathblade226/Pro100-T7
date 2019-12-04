using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Pro100_T7.Interfaces
{
    interface IClient
    {
        void ReceiveData(out byte[] data);
        void SendData(byte[] data);
        bool TryConnectToServer(EndPoint server);
    }
}
