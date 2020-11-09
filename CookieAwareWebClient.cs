using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PirateShip
{
    public class CookieAwareWebClient : WebClient
    {
        /// <summary>
        /// Time in milliseconds
        /// </summary>
        public int Timeout { get; set; }
        Uri _responseUri;

        public Uri ResponseUri
        {
            get { return _responseUri; }
        }
        public CookieAwareWebClient() : this(60000) { }

        public CookieAwareWebClient(int timeout)
        {
            this.Timeout = timeout;
        }
        public CookieContainer CookieContainer { get; set; } = new CookieContainer();

        protected override WebResponse GetWebResponse(WebRequest request)
        {
            WebResponse response = base.GetWebResponse(request);
            _responseUri = response.ResponseUri;
            return response;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            WebRequest request = base.GetWebRequest(address);
            HttpWebRequest webRequest = request as HttpWebRequest;
            
            if (request != null)
            {
                request.Timeout = this.Timeout;
                webRequest.ReadWriteTimeout = this.Timeout;
            }
            if (webRequest != null)
            {
                if (CookieContainer != null)
                {

                    webRequest.CookieContainer = CookieContainer;
                }
            }
            return request;
        }
    }
}
