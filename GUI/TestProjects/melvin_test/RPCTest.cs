using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading;
using System.Text;

namespace RabbitMQNet {
    public class RPCTest {
        private RabbitMQ.Client.ConnectionFactory factory = new RabbitMQ.Client.ConnectionFactory();
        protected RabbitMQ.Client.IConnection connection;
        protected IModel model;
        private System.Collections.Generic.IDictionary<Int32,TopicPerPortfolioThread>
                    mapOfThreadAndPortfolio = new System.Collections.Generic.Dictionary<Int32,TopicPerPortfolioThread>();
        private System.Collections.Generic.IDictionary<Int32,Thread>
                    mapOfThreadsPerPortfolio = new System.Collections.Generic.Dictionary<Int32,Thread>();

        protected string TOPIC_PREFIX = "AMP";


        public static void Main(string[] args) {
            RPCTest rPCTest = new RPCTest();
            rPCTest.doRpcTest();
        }

        public RPCTest() {
            //start the factory;
            Uri uriX = new Uri("amqp://guest:guest@172.16.143.199:5672");
            AmqpTcpEndpoint tcp = new AmqpTcpEndpoint(uriX);
            factory.Endpoint = tcp;
            connection = factory.CreateConnection();
            model = connection.CreateModel();
        }




        private void doRpcTest() {
            string corrId = "1";
            string replyTo = model.QueueDeclare();

            var otherHeader = new System.Collections.Generic.Dictionary<string,object>();
            otherHeader.Add("__TypeId__","com.amphora.pojo.PortfolioTopicRequest");


            IBasicProperties props1 = model.CreateBasicProperties();
            props1.CorrelationId = corrId;
            props1.ReplyTo = replyTo;
            props1.ContentType = "application/json";
            props1.DeliveryMode = 2;
            props1.Headers = otherHeader;

            //channel.exchangeDeclare("portfolio-exchange.1", "topic");
            //channel.queueDeclare("portfolio-exchange.1", false, false, false, null);
            QueueingBasicConsumer consumer = new QueueingBasicConsumer(model);
//            model.BasicConsume(replyTo,null,consumer);
            model.BasicConsume(replyTo,false,consumer);


            PortfolioTopicRequest topicRequest = new PortfolioTopicRequest();
            topicRequest.userId = "mramos";
            topicRequest.portfolioIds = new string[] { "58","59" };

            string jsonST = JsonConvert.SerializeObject(topicRequest);
            model.BasicPublish("portfolio-exchange","portfolio-exchange-route",props1,Encoding.UTF8.GetBytes(jsonST));

            string response = "";
            while (true) {

                BasicDeliverEventArgs delivery = (BasicDeliverEventArgs)consumer.Queue.Dequeue();
                if (delivery.BasicProperties.CorrelationId == corrId) {
                    response = Encoding.UTF8.GetString(delivery.Body);
                    PortfolioTopicResponse responseObj = JsonConvert.DeserializeObject<PortfolioTopicResponse>(response);

                    foreach (var item in responseObj.portfolioToExchange) {

                        TopicPerPortfolioThread t1 = new TopicPerPortfolioThread(connection,item.Value);
                        Thread oThread = new Thread(new ThreadStart(t1.run));
                        // keep for safe keeping.. I am not listening to the channel.
                        mapOfThreadAndPortfolio.Add(item.Key,t1);
                        mapOfThreadsPerPortfolio.Add(item.Key,oThread);
                        oThread.Start();
                    }

                }
                break;
            }

            System.Console.WriteLine("Response =" + response);


            //Sleep the main thread..
            Thread.Sleep(10);


            //Awoke, kill the topic channel.
            // assume I know the key.
            TopicPerPortfolioThread toKill = null;
            mapOfThreadAndPortfolio.TryGetValue(59,out toKill);

            if (toKill != null) {
                mapOfThreadAndPortfolio.Remove(59); //ensure remove.
                toKill.stop(); // stop the loop.
            }

            Thread t2 = null;
            mapOfThreadsPerPortfolio.TryGetValue(59,out t2);

            //https://msdn.microsoft.com/en-us/library/aa645740%28v=vs.71%29.aspx
            if (t2 != null) {
                mapOfThreadsPerPortfolio.Remove(59); //ensure remove to avoid memory leaks.
                t2.Abort(); //abort the thread to force it to exit.
            }


        }
    }


    public class TopicPerPortfolioThread {

        private IModel channel;
        private string topic;
        private volatile Boolean started;
        public TopicPerPortfolioThread(RabbitMQ.Client.IConnection connection,string topic) {
            this.topic = topic;
            channel = connection.CreateModel(); // you are passing the connection here to create the channel/model, you can just pass the model/channel too if you wanted to.
            started = true;
        }

        public Boolean isThreadStarted() {
            return started;
        }

        public void stop() {
            started = false;
        }

        public void run() {

            //channel.ExchangeDeclare(this.topic, "topic", false, false, true, false, false, null);	
            try {
                string queueName = channel.QueueDeclare();
//                channel.QueueBind(queueName,this.topic,"",false,null);
                channel.QueueBind(queueName,this.topic,string.Empty);
                var consumer1 = new QueueingBasicConsumer(channel);
//                channel.BasicConsume(queueName,null,consumer1);
                channel.BasicConsume(queueName,false,consumer1);

                while (started) {
                    var ea = (BasicDeliverEventArgs)consumer1.Queue.Dequeue();
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine("Topic=" + this.topic + " [x] {0}",message); // do sometthing with the message....
                }
            } catch (Exception e) {
                Console.WriteLine("Exception" + e);
            }
            //you should not reach this point if you do, then it means that the thread is set to stop.
            // from here on you are dropping the channel from connection.
            channel.Close();
            channel = null;

        }

    }


    public class PortfolioTopicResponse {
        public System.Collections.Generic.IDictionary<Int16,string> portfolioToExchange { get; set; }
    }

    public class PortfolioTopicRequest {

        public string userId { get; set; }
        public string[] portfolioIds { get; set; }

    }
}