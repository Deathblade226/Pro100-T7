using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Pro100_T7.Interfaces
{
    interface IServer
    {
        void SendData(byte[] data);
        void TryConnectClient(object sender, ElapsedEventArgs e);
    }
}
