#define USE_MELVINS_WAY
#undef CREATE_URI
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NSRMCommon;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Framing;

// select * from ictspass.topic_portfolio_mappings order by port_num

namespace NSTest {

    class RPCClient : IDisposable {
        #region fields
        IConnection connection;
        IModel model;
        #endregion

        #region constants
        #region connection constants
        const string QUSER = "guest";
        const string QPASS = "guest";
        const string HOST_NAME = "172.16.143.199";
        const int HOST_PORT = 5672;
        const string URI_PREFIX = "amqp://";
        #endregion

        #region port-specific constants
        const string JSON_TYPE = "application/json";
        const string JAVA_REQUEST_CLASS_NAME = "com.amphora.pojo.PortfolioTopicRequest";
        const string PORT_EXCHANGE = "portfolio-exchange";
        const string PORT_ROUTING_KEY = "portfolio-exchange-route";
        #endregion

        #endregion

        #region ctor
        public RPCClient() {
            ConnectionFactory factory;

            try {
#if CREATE_URI
                string content;
                Uri uri;

                content = URI_PREFIX + QUSER + ":" + QPASS + "@" + HOST_NAME + ":" + HOST_PORT;
                factory = new ConnectionFactory();
                factory.Endpoint = new AmqpTcpEndpoint(uri = new Uri(content));
#else
                factory = new ConnectionFactory();
                factory.HostName = HOST_NAME;
                if (HOST_PORT > 0) factory.Port = HOST_PORT;
                if (!string.IsNullOrEmpty(QUSER)) factory.UserName = QUSER;
                if (!string.IsNullOrEmpty(QPASS)) factory.Password = QPASS;
#endif
                connection = factory.CreateConnection();
                model = connection.CreateModel();
            } catch (Exception ex) {
                Util.show(MethodBase.GetCurrentMethod(),ex);
            }
        }
        #endregion

        /// <summary>Request spefic portfolio information.</summary>
        /// <param name="userName">a <see cref="string"/> containing the userid requesting this information.</param>
        /// <param name="desiredPorts">an <see cref="Array"/> of <see cref="int"/> containing portfolios to query.</param>
        /// <returns></returns>
        internal string grabPorts(string userName,int[] desiredPorts) {
            IBasicProperties props;
            string jsonValue;

            if (model == null)
                throw new InvalidOperationException("model is null!");
            Util.show(MethodBase.GetCurrentMethod(),"creating properties");
            props = setupProperties(JAVA_REQUEST_CLASS_NAME);
            Util.show(MethodBase.GetCurrentMethod(),"grabbing ports");
            return readPortInfo(props,new PortfolioRequest(userName,desiredPorts).toBytes(out jsonValue),PORT_EXCHANGE,PORT_ROUTING_KEY);
        }

        /// <summary>Extract hard-coded portfolio information.</summary>
        /// <returns>a <b>JSON</b>-conforming string, containing portfolio information.</returns>
        internal string grabPorts() {
            return grabPorts("mramos",new int[] { 53,54,55,58,59 });
        }

        /// <summary>Establish properties required to make this work.</summary>
        /// <param name="transportJavaClass"></param>
        /// <returns></returns>
        IBasicProperties setupProperties(string transportJavaClass) {
            IBasicProperties props = model.CreateBasicProperties();

            props.CorrelationId = Guid.NewGuid().ToString();
            props.ReplyTo = model.QueueDeclare().QueueName;
            props.ContentType = JSON_TYPE;

            if (props.Headers == null)                                                                        //
                props.Headers = new Dictionary<string,object>();
            if (!string.IsNullOrEmpty(transportJavaClass))
                props.Headers.Add("__TypeId__",transportJavaClass);
            return props;
        }

        /// <summary>Extract portfolio information, given a <see cref="PortfolioRequest"/>.</summary>
        /// <param name="props"></param>
        /// <param name="pr"></param>
        /// <param name="exchange"></param>
        /// <param name="routingKey"></param>
        /// <returns></returns>
        string readPortInfo(IBasicProperties props,PortfolioRequest pr,string exchange,string routingKey) {
            return readPortInfo(props,pr.toBytes(),exchange,routingKey);
        }

        /// <summary>Extract portfolio information, given an <see cref="Array"/> of <see cref="byte"/>, which is an encoded JSON object.</summary>
        /// <param name="props"></param>
        /// <param name="body"></param>
        /// <param name="exchange"></param>
        /// <param name="routingKey"></param>
        /// <returns></returns>
        string readPortInfo(IBasicProperties props,byte[] body,string exchange,string routingKey) {
            BasicDeliverEventArgs delivery;
            QueueingBasicConsumer consumer;
            string response = null;

            if (props == null)
                throw new ArgumentNullException("props","IBasicProperties is null!");
            Util.show(MethodBase.GetCurrentMethod(),"creating consumer");
            consumer = new QueueingBasicConsumer(model);
            model.BasicConsume(props.ReplyTo,false,consumer);
            Util.show(MethodBase.GetCurrentMethod(),"publishing request");

            model.BasicPublish(exchange,routingKey,props,body);
            Util.show(MethodBase.GetCurrentMethod(),"beginning consumer-loop.");
            while (true) {
                if ((delivery = consumer.Queue.Dequeue() as BasicDeliverEventArgs) != null &&
                    delivery.BasicProperties.CorrelationId.Equals(props.CorrelationId)) {
                    response = Encoding.UTF8.GetString(delivery.Body);
                    break;
                }
            }
            Util.show(MethodBase.GetCurrentMethod(),"done with consumer-loop.");
            return response;
        }

        public void Dispose() {
            if (model != null) {

                try {
                    model.Close(Constants.ReplySuccess,"OK");
                    model.Dispose();
                } catch (Exception ex) {
                    Util.show(MethodBase.GetCurrentMethod(),ex);
                } finally {
                    model = null;
                }
            }
            if (connection != null) {
                try {
                    connection.Close(Constants.ReplySuccess,"OK");
                    connection.Dispose();
                } catch (Exception ex) {
                    Util.show(MethodBase.GetCurrentMethod(),ex);
                } finally {
                    connection = null;
                }
            }
        }

        class MyTraceWriter : ITraceWriter {

            public MyTraceWriter() : this(TraceLevel.Error) { }

            public MyTraceWriter(TraceLevel aTraceLevel) {
                this.LevelFilter = aTraceLevel;
            }

            public TraceLevel LevelFilter {
                get;
                 set;
            }

            void ITraceWriter.Trace(TraceLevel level,string message,Exception ex) {
                Debug.Print("[" + level + "] " + message + ":" + Util.makeErrorMessage(ex,false));
            }
        }

        internal void listenToExchanges(List<string> channels) {
            QueueingBasicConsumer consumer;
            BasicDeliverEventArgs delivery;
            string message,routingKey,queueName;
            JsonSerializer js = new JsonSerializer();
            JSPosition anObj;
            js.TraceWriter = new MyTraceWriter();
            int numExchanges = 0;

            try {
                queueName = model.QueueDeclare().QueueName;
                foreach (string anExchange in channels) {
                    try {
                        model.QueueBind(queueName,anExchange,string.Empty);
                        Console.WriteLine("Adding exchange '" + anExchange + "'.");
                        numExchanges++;
                    } catch (Exception ex) {
                        Util.show(MethodBase.GetCurrentMethod(),ex);
                    }
                }

                if (numExchanges > 0) {

                    consumer = new QueueingBasicConsumer(model);
                    model.BasicConsume(queueName,true,consumer);

                    while (true) {
                        if ((delivery = consumer.Queue.Dequeue() as BasicDeliverEventArgs) != null) {
                            anObj = extractPosition(delivery,js,Encoding.UTF8,out message);
                            routingKey = delivery.RoutingKey;
                            Debug.Print(message);
                        }
                    }
                }
            } catch (Exception ex) {
                Util.show(MethodBase.GetCurrentMethod(),ex);
            }
        }

        JSPosition extractPosition(BasicDeliverEventArgs delivery,JsonSerializer js,Encoding encoding,out string message) {
            JSPosition anObj=null;

            message = encoding.GetString(delivery.Body);
            using (StringReader sr = new StringReader(message)) {
                using (JsonTextReader jtr = new JsonTextReader(sr)) {
                    try {
                        anObj = js.Deserialize(jtr,typeof(JSPosition)) as JSPosition;
                    } catch (Exception ex) {
                        Util.show(MethodBase.GetCurrentMethod(),ex);
                    }
                }
            }
            return anObj;
        }
    }

    class JSPosition {

        public int posNum { get; set; }
        public double quantity { get; set; }
        public string commodityCode { get; set; }
        public string marketCode { get; set; }
        public string tradingPeriod { get; set; }
        public string formulaName { get; set; }
        public int formulaNum { get; set; }
        //     public int posNum { get; set; }
        public int transactionId { get; set; }
        /*
         * {
         *      "posNum":648,
         *      "quantity":-1.09948814E-8,
         *      "commodityCode":"GASOIL",
         *      "marketCode":"IPE",
         *      "tradingPeriod":"200409",
         *      "formulaName":"",
         *      "formulaNum":0,
         *      "transactionId":519362209
         * }
         * */

    }

    class RPC {
        public static void Main() {
            RPCClient client = null;
            PortfolioRequest anObj = null;
            string result,exchange;
            List<string> exchanges;
            TextWriterTraceListener twtl=new TextWriterTraceListener(Console.Out);
            int exitCode = 0;

            Debug.AutoFlush = false;
            Debug.Listeners.Add(twtl);

            try {
                client = new RPCClient();
                Util.show(MethodBase.GetCurrentMethod());
                result = client.grabPorts("rcousens",new int[] { 53,54,55,56,57,58,59 });
                if (string.IsNullOrEmpty(result))
                    Console.WriteLine("nothing found");
                else {
                    using (StringReader sr = new StringReader(result)) {
                        using (JsonTextReader jtr = new JsonTextReader(sr)) {
                            JsonSerializer js = new JsonSerializer();
                            try {
                                anObj = js.Deserialize(jtr,typeof(PortfolioRequest)) as PortfolioRequest;
                                exchanges = new List<string>();
                                foreach (var avar in anObj.portfolioToExchange.Keys) {
                                    if (!exchanges.Contains(exchange = anObj.portfolioToExchange[avar]))
                                        exchanges.Add(exchange);
                                }
                                if (exchanges.Count > 0) {
                                    Debug.Print("have exchanges");
                                    client.listenToExchanges(exchanges);
                                }
                            } catch (Exception ex) {
                                Util.show(MethodBase.GetCurrentMethod(),ex);
                                exitCode = 1;
                            }
                            Console.WriteLine("found: " + result + "[" + (anObj == null ? "-NULL-" : anObj.ToString() + "]"));
                        }
                    }
                }

            } catch (Exception ex) {
                Util.show(MethodBase.GetCurrentMethod(),ex);
                exitCode = 2;
            } finally {
                if (client != null)
                    client.Dispose();
            }
            Environment.Exit(exitCode);
        }
    }
}