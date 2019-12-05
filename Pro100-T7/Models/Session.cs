using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pro100_T7.Models
{
    public static class Session
    {
        public static bool IsServer { get; set; } = false;
        public static Server ServerWhereApplicable { get; set; } = null;

        public static Client CurrentClientSession { get; set; } = null;
        public static bool BuildOnlineSession { get; set; } = false;
        public static bool IsOnlineSession { get; set; } = false;

        public static void Initialize(bool online, bool isServer = false)
        {
            BuildOnlineSession = online;
            IsServer = isServer;
        }

        public static void Build(Client c, Server s = null)
        {
            CurrentClientSession = c;
            IsOnlineSession = true;
            if (IsServer && s != null) ServerWhereApplicable = s;
            if (s == null) IsServer = false;
        }
    }
}
