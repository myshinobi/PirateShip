using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PirateShip
{
    public class Booty
    {
        public string Body;
        public HttpStatusCode? statusCode;
        public WebExceptionStatus? exceptionStatus;
        public HttpWebResponse webResponse;
        public PirateShip.DownloadMethod downloadMethod;
        public byte[] data;
        public string errorMessage;
        public Exception exception;
        public bool invalidURL;
        public bool redirected;
        public Uri redirectedUri;
        //public IEnumerable<string> SetCookies;

    }
}
