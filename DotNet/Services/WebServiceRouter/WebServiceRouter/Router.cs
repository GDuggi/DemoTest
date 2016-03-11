using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Web;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Net.Http;
using System.Collections.Specialized;
using log4net;
using System.Text.RegularExpressions;
using System.Configuration;
using WebServiceRouter.Config;
using System.Threading;
using System.ServiceModel;

/*
 * This program is a SOAP man-in-the-middle that can examine a SOAP payload and 
 * direct the call to another server.  
 * 
 * It looks at The SOAPAction and a SOAP Body element called tradingSystemCode
 * to route calls to an identical Web Service.
 * 
 * The rules to control the routing and the listening port can be found in the 
 * App.config file.
 * 
 * Terminology - all from the *perspective of the Router*:
 * 
 *  Router: this app
 *
 *  Client: the app that calls the router that is redirected to a service
 *    clien-in: the SOAP request from the Client
 *    client-out: the SOAP response from the Service forwarded back to the Client
 *    
 *  Service: the app hosting the SOAP service
 *    service-out: the SOAP request that originated from the client-in and sent to the Service
 *    service-in: the SOAP response from the Service, send this back to the Client
 *    
 *  Message flow:
 *    Client => Router:client-in => Router:service-out => Service => Router:service-in => Router:client-out => Client
 * 
 */

namespace WebServiceRouter
{
    public class Router
    {
        private static readonly String[] NO_COPY_HEADERS = { "Content-Length", "Content-Type", "Connection" };
        public static readonly String SOAP_ACTION = "SOAPAction";
        public static readonly Dictionary<String, HttpMethod> httpMethodMap = new Dictionary<String, HttpMethod>()
        {
            { "DELETE ", HttpMethod.Delete },
            { "GET", HttpMethod.Get },
            { "HEAD  ", HttpMethod.Head },
            { "OPTIONS", HttpMethod.Options },
            { "POST", HttpMethod.Post },
            { "PUT", HttpMethod.Put },
            { "TRACE", HttpMethod.Trace },
        };

        volatile bool stopRequest;
        static XmlNamespaceManager namespaceManager;
        List<WebServiceRouterRuleConfig> rules;
        String listenMask;

        static Router()
        {
            namespaceManager = new XmlNamespaceManager(new NameTable());
            namespaceManager.AddNamespace("cnf", "http://cnf/Integration");
            namespaceManager.AddNamespace("cnfMgr", "http://cnf/ConfirmationsManager");
            namespaceManager.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/");
        }

        public Router()
        {
            var webServiceRouterConfigSection = ConfigurationManager.GetSection("webServiceRouterConfig") as WebServiceRouterConfigSection;
            if (webServiceRouterConfigSection == null)
            {
                throw new ConfigurationErrorsException("No configuration found in the app.config");
            }
            init(webServiceRouterConfigSection);
        }

        public Router(WebServiceRouterConfigSection webServiceRouterConfigSection)
        {
            init(webServiceRouterConfigSection);
        }

        public void init(WebServiceRouterConfigSection webServiceRouterConfigSection)
        {

            listenMask = webServiceRouterConfigSection.listenMask;

            rules = new List<WebServiceRouterRuleConfig>();
            var x = webServiceRouterConfigSection.RuleConfigurations;
            foreach (WebServiceRouterRuleConfig rule in x)
            {
                rules.Add(rule);
            }

            Log.InfoFormat("Rules defined: {0}\n\t{1}", rules.Count, String.Join("\n\t", rules));

            if (isEmpty(listenMask))
                throw new ConfigurationErrorsException("listenMask not set");

            if (rules.Count == 0)
                throw new ConfigurationErrorsException("At least one rule must be defined");
        }

        public void Start()
        {
            try
            {
                stopRequest = false;
                HttpListener listener = new HttpListener();
                listener.Prefixes.Add(listenMask);
                listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
                listener.Start();
                Log.InfoFormat("Listening on {0}", String.Join(", ", listener.Prefixes));

                while (!stopRequest)
                {
                    IAsyncResult result = listener.BeginGetContext(new AsyncCallback(ListenerCallback), listener);
                    //result.AsyncWaitHandle.WaitOne(1000, false);
                    Thread.Sleep(1000);                    
                }
                Log.Info("Listener stopped");
                listener.Close();
            }
            catch (Exception e)
            {
                Log.Fatal("Error in message processing loop", e);           
            }
        }

        public void Stop()
        {
            stopRequest = true;
        }

        public void ListenerCallback(IAsyncResult result)
        {
            HttpListenerContext context = null;
            try
            {
                HttpListener listener = (HttpListener)result.AsyncState;

                // Call EndGetContext to complete the asynchronous operation.
                context = listener.EndGetContext(result);
                ProcessHttp(context);
            }
            catch (Exception e)
            {
                Log.Error("Failed in listener callback", e);
                replyError(context, e);
            }

        }

        public void ProcessHttp(HttpListenerContext context)
        {
            try
            {
                HttpListenerRequest request = context.Request;

                // Get SOAP Action header
                String soapAction = request.Headers.Get(SOAP_ACTION);

                // Get optional XML element tradingSystemCode
                StreamReader stream = new StreamReader(request.InputStream);
                String s = stream.ReadToEnd();

                Log.InfoFormat("ProcessHttp: got request from {0}\nHeaders:\n{1}\nPayload:\n{2}",
                    context.Request.RemoteEndPoint.ToString(),
                    HeadersToString(request.Headers),
                    s);

                String url = getRoute(soapAction, s);

                if (url != null)
                {
                    String endpoint = url + request.RawUrl;
                    forwardTo(endpoint, soapAction, s, context);
                }
                else
                {
                    throw new RouterException(
                        String.Format("Failed to determine route for request from {0}\nHeaders:\n{1}\nPayload:\n{2}",
                        context.Request.RemoteEndPoint.ToString(),
                        HeadersToString(request.Headers),
                        s)
                        );
                }
            }
            catch (Exception e)
            {
                Log.Error("Failed to process request", e);
                throw e;
            }

        }

        // 
        // String forwardUrl = "http://localhost:8080/Counterparty.svc";
        //
        public String getRoute(String soapAction, String payload)
        {

            // Cleanup the SoapAction, removing leading and trailing spaces and quotes
            if (soapAction != null)
            {
                soapAction = soapAction.Trim();
                if (soapAction.StartsWith("\""))
                    soapAction = soapAction.Substring(1);
                if (soapAction.EndsWith("\""))
                    soapAction = soapAction.Substring(0, soapAction.Length - 1);
            }

            // Extract the tradingSystemCode from the XML
            String forwardUrl = null;
            String xpathValue = null;

            try
            {
                XDocument xmlInput = XDocument.Parse(payload);

                var xpath = xmlInput.XPathSelectElement("//cnf:tradingSystemCode[1]", namespaceManager);
                if(xpath==null)
                    xpath = xmlInput.XPathSelectElement("//cnfMgr:tradingSystemCode[1]", namespaceManager);
                if (xpath == null)
                    xpath = xmlInput.XPathSelectElement("tradingSystemCode[1]", namespaceManager);
                if (xpath != null)
                    xpathValue = xpath.Value;
            }
            catch (Exception e)
            {
                Log.InfoFormat("Failed to parse payload as XML: {0}", e.Message);
            }

            Log.InfoFormat("Looking for a rule to match soapAction={0}, tradingSystemCode={1}",
                toS( soapAction ), toS( xpathValue) );

            // Find a matching rule
            foreach (WebServiceRouterRuleConfig r in rules)
            {
                forwardUrl = matchRule(r, soapAction, xpathValue);
                if (forwardUrl != null)
                {
                    break;
                }
            }

            if (forwardUrl == null)
                Log.WarnFormat("Failed to locate url for getRoute( soapAction={0}, tradingSystemCode={1},\nPayload={2})",
                    toS(soapAction),  toS( xpathValue), toS(payload));

            return forwardUrl;
        }

        public void forwardTo(String url, String soapAction, String payload, HttpListenerContext context)
        {
            
            // Copy headers from original request
            var clientRequest = context.Request;
            var request = new HttpRequestMessage();          
            request.Headers.Clear();
            foreach (var k in clientRequest.Headers.AllKeys )
            {
                try
                {
                    if( validHeader( k ))
                        request.Headers.Add(k, clientRequest.Headers[k]);
                }
                catch (InvalidOperationException e)
                {
                    Log.DebugFormat("Did not add header {0}={1}", k, clientRequest.Headers[k]);
                }
            }

            request.Method = convertMethod( clientRequest.HttpMethod );
            request.Content = convertContentType(payload, clientRequest.ContentType);

            Log.InfoFormat("Forwarding to {0}\nHeaders:\n{1}\nPayload:\n{2}\n", toS( url ), toS( HeadersToString(request.Headers) ), toS( payload) );

            String responsePayload = null;

            var client = new HttpClient();
            client.BaseAddress = new Uri(url);
           
            client.SendAsync(request).ContinueWith((requestTask) =>
            {
                try
                {
                    if (!requestTask.IsFaulted)
                    {
                        HttpResponseMessage response = requestTask.Result;

                        if (response.IsSuccessStatusCode)
                        {
                            Log.InfoFormat("Got response from {0}, Headers:\n{1}", toS( url ), toS( HeadersToString(response.Headers)) );                      

                            // Read response asynchronously 
                            response.Content.ReadAsStringAsync().ContinueWith(
                                (readTask) =>
                                {
                                    responsePayload = readTask.Result;
                                    Log.InfoFormat("Web Service returned:\n{0}", responsePayload);
                                    reply(context, response.Headers, responsePayload);
                                });
                        }
                        else
                        {
                            throw new RouterException(
                                String.Format("Response from service indicated an error: {0}: {1}, Request Message={2}",
                                response.StatusCode, response.ReasonPhrase, response.RequestMessage)) {StatusCode= response.StatusCode};
                        }
                    }
                    else
                    {
                        throw new RouterException("Request to service failed", requestTask.Exception);
                    }
                }
                catch (Exception e)
                {
                    Log.Error("ERROR", e);
                    replyError(context, e);
                }
            });

        }

        public void reply(HttpListenerContext context, System.Net.Http.Headers.HttpResponseHeaders headers, String responsePayload)
        {
            try
            {
                Log.InfoFormat("Sending reply:\nHeaders:\n{0}\nPayload:\n{1}", toS( HeadersToString(headers) ), toS( responsePayload) );

                // Obtain a response object.
                HttpListenerResponse response = context.Response;               
                // Construct a response.            

                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responsePayload);
                // Get a response stream and write the response to it.
                response.ContentLength64 = buffer.Length;
                response.ContentType = context.Request.ContentType;
                response.ContentEncoding = context.Request.ContentEncoding;              
                System.IO.Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);

                output.Close();
                context.Response.Close();
            }
            catch (Exception e)
            {
                Log.Error("Failed to return the reply", e);
                throw e;
            }
        }

        public void replyError(HttpListenerContext context, Exception e )
        {
            // Obtain a response object.
            HttpListenerResponse response = context.Response;

            if (e is RouterException && (e as RouterException).StatusCode != null && (e as RouterException).StatusCode!=0)
            {
                response.StatusCode = (int)(e as RouterException).StatusCode;
                response.StatusDescription = (e as RouterException).StatusCode.ToString();
            }
            else
            {
                // Construct a response.            
                response.StatusCode = 500;
                response.StatusDescription = "InternalServerError";
            }
            //byte[] buffer = System.Text.Encoding.UTF8.GetBytes(e.ToString());
            //// Get a response stream and write the response to it.
            //response.ContentLength64 = buffer.Length;
            //response.ContentType = context.Request.ContentType;
            //response.ContentEncoding = context.Request.ContentEncoding;
            //System.IO.Stream output = response.OutputStream;
            //output.Write(buffer, 0, buffer.Length);

            //output.Close();
            response.Close();
        }



        private HttpContent convertContentType(string payload, string p)
        {
            if (isEmpty(payload) || isEmpty(p))
                return null;
            else
                return new StringContent(payload, Encoding.UTF8, "text/xml");
        }

        private HttpMethod convertMethod(String p)
        {
            if (httpMethodMap.ContainsKey(p))
                return httpMethodMap[p];
            else
                return HttpMethod.Post;
        }

        public String matchRule(WebServiceRouterRuleConfig rule, String soapAction, String tradingSystemCode)
        {
            // If our rule has a soapAction and it doesn't match then give up.
            if (!isEmpty(rule.soapAction) && !rule.soapAction.Equals(soapAction))
                return null;

            // if out rule has a tradingSystemCode and it doesn't match then give up
            if (!isEmpty(rule.tradingSystemCode) && !rule.tradingSystemCode.Equals(tradingSystemCode))
                return null;

            Log.InfoFormat("Found rule: soapAction={0}, tradingSystemCode={1}, url={2}",
                  toS(rule.soapAction) ,
                  toS(rule.tradingSystemCode) ,
                  toS(rule.url));

            return rule.url;
        }

        private static bool isEmpty(String s)
        {
            return s == null || s.Equals("");
        }

        private static String toS(String s)
        {
            return s == null ? "<null>" : (s.Equals( "" ) ? "<empty>" : s );
        }

        private static bool validHeader(string k)
        {
            return !isEmpty(k) && !NO_COPY_HEADERS.Contains(k);
        }
        
        private static Byte[] GetBytes(string str)
        {
            Byte[] bytes = new Byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        private static string GetString(Byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        private static String HeadersToString(System.Collections.Specialized.NameValueCollection headers)
        {
            return String.Join("\n", headers.AllKeys.Select(x =>
                    String.Format("\t{0}={1}", x, String.Join(",", headers[x])))
                    );
        }

        private static String HeadersToString(System.Net.Http.Headers.HttpRequestHeaders headers)
        {
            return String.Join("\n", headers.Select(x =>
                    String.Format("\t{0}={1}", x.Key, String.Join(",", x.Value)))
                    );
        }

        public String HeadersToString(System.Net.Http.Headers.HttpResponseHeaders headers)
        {
            return String.Join("\n", headers.Select(x =>
                    String.Format("\t{0}={1}", x.Key, String.Join(",", x.Value)))
                    );
        }

        private static ILog Log
        {
            get { return LogManager.GetLogger(typeof(Router)); }
        }


        public class RouterException : Exception
        {
            public RouterException() : base() { }
            public RouterException( String msg ) : base(msg) { }
            public RouterException( String msg, Exception e ) : base(msg, e ) { } 
        
           public HttpStatusCode StatusCode { get; set; }

        }

    }

     
}
