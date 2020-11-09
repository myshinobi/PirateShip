using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace PirateShip
{
    public class PirateShip
    {
        private Proxy _proxy;
        public Proxy proxy
        {
            get
            {
                return _proxy;
            }
            set
            {
                _proxy = value;
                if (_proxy != null)
                {

                    int port;
                    int.TryParse(_proxy.Port, out port);

                    WebProxy webProxy = new WebProxy(_proxy.IPv4, port);

                    if (!string.IsNullOrEmpty(_proxy.username))
                    {
                        ICredentials credentials = new NetworkCredential(_proxy.username, _proxy.password);


                        webProxy.Credentials = credentials;

                    }
                    client.Proxy = webProxy;

                }
            }
        }
        public Uri url;
        public Method method = Method.GET;

        public Session GetSession()
        {
            return new Session(this);
        }

        public HttpStatusCode? statusCode;
        public WebExceptionStatus? exceptionStatus;
        public HttpWebResponse webResponse;

        public DownloadMethod downloadMethod = DownloadMethod.String;
        public enum DownloadMethod
        {
            String,
            Data
        }
        public enum Method
        {
            GET,
            POST
        }
        public IEnumerable<KeyValuePair<string, string>> postData;
        //public IEnumerable<string> cookies;
        public static string DefaultUserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_2) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/76.0.3809.132 Safari/537.36";
        public IEnumerable<KeyValuePair<string, string>> headers;
        public string postContent;
        public static bool debug = false;
        public CookieAwareWebClient client = new CookieAwareWebClient();
        public CookieContainer cookieContainer
        {
            get
            {
                return client.CookieContainer;
            }

            set
            {
                client.CookieContainer = value;
            }
        }
        //public CookieContainer cookieContainer;

        public CookieCollection GetCookies()
        {
            return cookieContainer.GetCookies(url);
        }

        public Booty lastBooty;
        //public HttpRequestMessage message = null;
        //public HttpResponseMessage response = null;
        public string responseBody;
        public byte[] responseData;

        private bool _freeze = true;
        public bool freeze
        {
            get
            {
                if (debug)
                {
                    return _freeze;
                }
                return false;
            }

            set
            {
                if (debug)
                {
                    _freeze = true;
                    Debugger debugger = new Debugger(this);
                    debugger.ShowDialog();
                    _freeze = false;
                }
                else
                {
                    _freeze = false;
                }
            }
        }

        //public PirateShip()
        //{

        //}

        public PirateShip(Proxy proxy)
        {
            this.proxy = proxy;


        }

        public Booty PlunderWebsite()
        {
            return PlunderWebsiteTask();
        }

        public static byte[] ReadFully(Stream input)
        {
            input.Position = 0;
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }

        public static void WriteCookiesToDisk(string file, CookieContainer cookieJar)
        {

            using (Stream stream = File.Create(file))
            {
                try
                {
                    Console.Out.Write("Writing cookies to disk... ");
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, cookieJar);
                    Console.Out.WriteLine("Done.");
                }
                catch (Exception e)
                {
                    Console.Out.WriteLine("Problem writing cookies to disk: " + e.GetType());
                }
            }
        }

        public static CookieContainer ReadCookiesFromDisk(string file)
        {

            try
            {
                using (Stream stream = File.Open(file, FileMode.Open))
                {
                    Console.Out.Write("Reading cookies from disk... ");
                    BinaryFormatter formatter = new BinaryFormatter();
                    Console.Out.WriteLine("Done.");
                    return (CookieContainer)formatter.Deserialize(stream);
                }
            }
            catch (Exception e)
            {
                Console.Out.WriteLine("Problem reading cookies from disk: " + e.GetType());
                return new CookieContainer();
            }
        }


        public string GetSessionCookiesFilePath(string name)
        {

            string path = Directory.GetCurrentDirectory();
            return path + "\\Sessions\\" + name + "\\"+name+".cookies";
        }


        public string GetSessionShipFilePath(string name)
        {

            string path = Directory.GetCurrentDirectory();
            return path + "\\Sessions\\" + name + "\\" + name + ".ship";
        }


        public void SaveSession(string name)
        {
            string shipPath = GetSessionShipFilePath(name);
            FileInfo fileInfo = new FileInfo(shipPath);
            Directory.CreateDirectory(fileInfo.DirectoryName);

            Session session = GetSession();
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(session);//new string[] { this.url.AbsolutePath,});
            File.WriteAllText(shipPath, json);

            WriteCookiesToDisk(GetSessionCookiesFilePath(name), cookieContainer);

        }

        public bool LoadSession(string name)
        {
            string shipPath = GetSessionShipFilePath(name);
            FileInfo fileInfo = new FileInfo(shipPath);

            if (File.Exists(shipPath))
            {
                string json = File.ReadAllText(shipPath);
                Session ship = Newtonsoft.Json.JsonConvert.DeserializeObject<Session>(json);
                this.url = new Uri(ship.url);
                cookieContainer = ReadCookiesFromDisk(GetSessionCookiesFilePath(name));

                return true;
            }

            return false;
        }

        public static Booty PlunderWebsite(string url, Proxy proxy = null)
        {
            return PlunderWebsite(new Uri(url), proxy);
        }

        public static Booty PlunderWebsite(Uri url, Proxy proxy, Method method = Method.GET, IEnumerable<KeyValuePair<string,string>> postData = null)
        {
            PirateShip pirateShip = new PirateShip(proxy);
            pirateShip.url = url;
            pirateShip.method = method;
            pirateShip.postData = postData;

            return pirateShip.PlunderWebsite();
        }


        public static void PrintCookies(CookieContainer cookieJar)
        {
            try
            {
                Hashtable table = (Hashtable)cookieJar
                    .GetType().InvokeMember("m_domainTable",
                    BindingFlags.NonPublic |
                    BindingFlags.GetField |
                    BindingFlags.Instance,
                    null,
                    cookieJar,
                    new object[] { });


                foreach (var key in table.Keys)
                {
                    // Look for http cookies.
                    try
                    {

                        if (cookieJar.GetCookies(
                            new Uri(string.Format("http://{0}/", key))).Count > 0)
                        {
                            Console.WriteLine(cookieJar.Count + " HTTP COOKIES FOUND:");
                            Console.WriteLine("----------------------------------");
                            foreach (Cookie cookie in cookieJar.GetCookies(
                                new Uri(string.Format("http://{0}/", key))))
                            {
                                Console.WriteLine(
                                    "Name = {0} ; Value = {1} ; Domain = {2}",
                                    cookie.Name, cookie.Value, cookie.Domain);
                            }
                        }
                    }
                    catch (Exception)
                    {

                        //Console.WriteLine(e);

                    }

                    // Look for https cookies
                    try
                    {
                        if (cookieJar.GetCookies(
    new Uri(string.Format("https://{0}/", key))).Count > 0)
                        {
                            Console.WriteLine(cookieJar.Count + " HTTPS COOKIES FOUND:");
                            Console.WriteLine("----------------------------------");
                            foreach (Cookie cookie in cookieJar.GetCookies(
                                new Uri(string.Format("https://{0}/", key))))
                            {
                                Console.WriteLine(
                                    "Name = {0} ; Value = {1} ; Domain = {2}",
                                    cookie.Name, cookie.Value, cookie.Domain);
                            }
                        }
                    }
                    catch (Exception)
                    {

                        //Console.WriteLine(e);
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public string GetParameters()
        {
            if (postData == null)
            {
                return "";
            }
            return string.Join("&", postData.Select(x => x.Key + "=" + HttpUtility.UrlEncode(x.Value)));
        }

        public static string[] validSchemes = new string[] {"http","https" };

        private Booty PlunderWebsiteTask()
        {
            statusCode = null;
            responseBody = "";
            responseData = null;

            bool wasRedirected = false;
            string errorMessage = "";
            Exception exception = null;
            bool invalidUrl = false;
            using (client)
            {
                // Call asynchronous network methods in a try/catch block to handle exceptions
                try
                {
                    if (!validSchemes.Contains(url.Scheme))
                    {
                        invalidUrl = true;
                        throw new Exception("Invalid scheme!");
                    }
                    if (headers!= null)
                    {
                        foreach (KeyValuePair<string,string> item in headers)
                        {
                            client.Headers[item.Key] = item.Value;
                        }
                    }
                    client.Headers.Add("user-agent", (proxy != null ? (string.IsNullOrEmpty(proxy.UserAgent) ? DefaultUserAgent : proxy.UserAgent) : DefaultUserAgent));
                    //client.Headers.Add("user-agent",DefaultUserAgent);
                    switch (method)
                    {
                        case Method.GET:
                            freeze = true;

                            switch (downloadMethod)
                            {
                                case DownloadMethod.String:
                                    responseBody = client.DownloadString(url);
                                    
                                    break;
                                case DownloadMethod.Data:
                                    responseData = client.DownloadData(url);
                                    break;
                                default:
                                    break;
                            }
                            break;

                        case Method.POST:
                            client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                            freeze = true;
                            responseBody = client.UploadString(url, GetParameters());

                            break;
                        default:
                            break;
                    }

                    if (client.ResponseUri != url)
                    {
                        wasRedirected = true;
                        throw new Exception("Url was redirected and result cannot be trusted.");
                    }

                    statusCode = HttpStatusCode.OK;
                }
                catch (WebException ex)
                {
                    exception = ex;
                    exceptionStatus = ex.Status;
                    webResponse = (ex.Response as HttpWebResponse);
                    HttpStatusCode? status = webResponse?.StatusCode;
                    statusCode = status;
                    
                    freeze = true;
                    errorMessage = ex.Message;
                    //throw ex;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    errorMessage = ex.Message;
                    exception = ex;
                }
                finally
                {

                    freeze = true;
                }
            }

            Booty booty = new Booty();
            booty.Body = responseBody;
            booty.data = responseData;
            booty.downloadMethod = downloadMethod;
            booty.statusCode = statusCode;
            booty.exceptionStatus = exceptionStatus;
            booty.webResponse = webResponse;
            booty.errorMessage = errorMessage;
            booty.exception = exception;
            booty.invalidURL = invalidUrl;

            booty.redirected = wasRedirected;
            if (booty.redirected)
            {
                booty.redirectedUri = client.ResponseUri;
            }

            if (webResponse != null)
            {

                if (string.IsNullOrEmpty(booty.Body) && booty.data == null)
                {
                    try
                    {

                        using (var reader = new StreamReader(webResponse.GetResponseStream()))
                        {
                            string result = reader.ReadToEnd(); // do something fun...
                            booty.Body = result;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
            else
            {
                if (string.IsNullOrEmpty(errorMessage)==false)
                {
                    Console.WriteLine("no webresponse: " + errorMessage+ ": "+url);
                }
            }

            this.lastBooty = booty;
            return booty;
        }

    }
}
