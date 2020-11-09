using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PirateShip
{
    public class Proxy
    {
        public string IPv4;
        public string Port;
        public bool isHttps;
        public string username;
        public string password;
        public string UserAgent;

        public static Proxy BuildPirateProxy(string proxy, string userAgent = "")
        {
            Proxy pirateProxy = new Proxy();
            pirateProxy.UserAgent = userAgent;
            if (proxy.Contains('@'))
            {
                string[] proxyDetails = proxy.Split('@');

                string[] proxyCred = proxyDetails[0].Split(':');
                string[] proxyURL = proxyDetails[1].Split(':');

                pirateProxy.username = proxyCred[0];
                pirateProxy.password = proxyCred[1];

                pirateProxy.IPv4 = proxyURL[0];
                pirateProxy.Port = proxyURL[1];

            }
            else
            {
                string[] proxyDetails = proxy.Split(':');

                pirateProxy.IPv4 = proxyDetails[0];
                pirateProxy.Port = proxyDetails[1];
            }

            return pirateProxy;
        }
    }
}
