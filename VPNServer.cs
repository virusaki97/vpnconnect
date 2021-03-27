using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;                                

namespace VPNClient
{
    public class VPNServer
    {
        public IPInfo ipinfo;
        public NetworkCredential credentials;

        public VPNServer(string VPNentry)
        {
            FromString(VPNentry);
        }

        public void FromString(string VPNentry)
        {
            var split = VPNentry.Split(' ');

            ipinfo = new IPInfo(split[0]);
            credentials = new NetworkCredential(split[1], split[2]);
        }

    }
}
