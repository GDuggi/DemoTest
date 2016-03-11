using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Apache.NMS;
//using Apache.NMS.Stomp;
using Apache.NMS.Util;
using Apache.NMS.Stomp;
 
namespace ApacheNMS_Test
{
    public partial class frmTestSimple : Form
    {
        public IConnectionFactory factory;
        public IConnection connection;
        public ISession session;
        public IDestination destination;
        public IMessageConsumer consumer;
        public IMessageProducer producer;
        public Uri connectUri;
        private TopicSubscriber topicSubscriber;
        private const string TOPIC_NAME = "sempra.ops.opsTracking.summary.update";
        private const string MAX_INACTIVITY_DURATION = "?wireFormat.maxInactivityDuration=0";
        private const string CLIENT_ID = "ifrankel.test.clientId";
        private const string CONSUMER_ID = "ifrankel.test.subscriber";

        public frmTestSimple()
        {
            InitializeComponent();
            Environment.SetEnvironmentVariable("is.hornetQ.client", "true");
            connectUri = new Uri("stomp:tcp://aff01inf01:61613");

            //IConnectionFactory factory = new ConnectionFactory(new Uri("stomp:tcp://aff01inf01:61613"));
            //IConnectionFactory factory = new ConnectionFactory(connectUri);
            //IConnection connection = factory.CreateConnection("sempra.ops.gs.service", "sempra");
            //ISession session = connection.CreateSession();

            //destination = SessionUtil.GetDestination(session, "topic://sempra.ops.opsTracking.summary.update" + MAX_INACTIVITY_DURATION);
            //consumer = session.CreateConsumer(destination);
            //connection.Start();

            //consumer.Listener += new MessageListener(OnMessage);
        }

        public void InitForm()
        {
            try
            {

                factory = new ConnectionFactory(connectUri);
                connection = factory.CreateConnection("sempra.ops.gs.service", "sempra");
                connection.ExceptionListener += new ExceptionListener(OnException);
                connection.Start();

                session = connection.CreateSession();
                
                //destination = SessionUtil.GetDestination(session, "topic://sempra.ops.opsTracking.summary.update" + MAX_INACTIVITY_DURATION);

                //This is the process that registers the subscriber on the server
                //consumer = session.CreateConsumer(destination);
                ////////////////////////////////////////

                //producer = session.CreateProducer(destination);
                //consumer.Listener += new MessageListener(OnMessage);
                
                topicSubscriber = new TopicSubscriber(session, TOPIC_NAME);
                topicSubscriber.OnMessageReceived += new MessageListener(OnMessage);
                topicSubscriber.Start(CONSUMER_ID);

                ////////////////////////////////////////
                //_connectionFactory = new ConnectionFactory(_connectUri, CLIENT_ID);
                //_connection = _connectionFactory.CreateConnection(Properties.Settings.Default.MessageServerUserId, Properties.Settings.Default.MessageServerPassword);
                //_connection.ExceptionListener += new ExceptionListener(OnException);
                //_connection.Start();
                //_session = _connection.CreateSession();
                //_session = _connection.CreateSession(AcknowledgementMode.AutoAcknowledge);

                //_topicSubscriber = new TopicSubscriber(_session, TOPIC_NAME);
                //_topicSubscriber.Start(CONSUMER_ID);
                //_topicSubscriber.OnMessageReceived += new MessageListener(OnMessage);
            }
            catch (Exception error)
            {
                MessageBox.Show("Error:" + error.Message);
            }
        }

        private void btnInitForm_Click(object sender, EventArgs e)
        {
            InitForm();
        }
        private void btnStartLstn_Click(object sender, EventArgs e)
        {
            //connection.Start();
        }

        private void btnStopLstn_Click(object sender, EventArgs e)
        {
            //if (connection.IsStarted)
            //    connection.Stop();
        }

        //Necessary to use delegate because of "Cross-thread operation not valid" error
        //writing to form control.
        delegate void OnMessageCallback(ITextMessage message);
        public void OnMessage(IMessage message)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.rtxtMsgRcv.InvokeRequired)
            {
                OnMessageCallback d = new OnMessageCallback(OnMessage);
                this.Invoke(d, new object[] { message });
            }
            else
            {
                try
                {
                    ITextMessage msg = (ITextMessage)message;
                    string txt = msg.Text;
                    rtxtMsgRcv.AppendText("Message Received " + timeStamp(DateTime.Now) + ":  " + txt + "\n");
                }
                catch (Exception e)
                {
                    MessageBox.Show("Exception receiving message: " + "\n" + e.Message.ToString());
                }
            }
        }

        public void OnException(Exception e)
        {
            MessageBox.Show("Exception receiving message: " + "\n" + e.Message.ToString());
        }

        private void frmTestMain_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void btnSendMsg_Click(object sender, EventArgs e)
        {
            try
            {
                string message = "Hello from C# Simple Test.";
                //var textMessage = producer.CreateTextMessage(message);
                //producer.Send(textMessage);
                //using (var publisher = new TopicPublisher(session, TOPIC_NAME))
                //{
                //    publisher.SendMessage(message);
                //}

                using (ISession session2 = connection.CreateSession())
                {
                    using (IMessageProducer producer = session2.CreateProducer(destination))
                    {
                        // Start the connection so that messages will be processed.
                        //connection.Start();
                        //producer.Persistent = true;
                        //producer.RequestTimeout = receiveTimeout;
                        //consumer.Listener += new MessageListener(OnMessage);

                        // Send a message
                        ITextMessage request = session2.CreateTextMessage("Hello World!");
                        //request.NMSCorrelationID = "abc";
                        //request.Properties["NMSXGroupID"] = "cheese";
                        //request.Properties["myHeader"] = "Cheddar";

                        producer.Send(request);

                        // Wait for the message
                        //semaphore.WaitOne((int)receiveTimeout.TotalMilliseconds, true);
                        //if (message == null)
                        //{
                        //    Console.WriteLine("No message received!");
                        //}
                        //else
                        //{
                        //    Console.WriteLine("Received message with ID:   " + message.NMSMessageId);
                        //    Console.WriteLine("Received message with text: " + message.Text);
                        //}
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception sending message: " + "\n" + ex.Message.ToString());
               
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public static String timeStamp(DateTime value)
        {
            return value.ToString("yyyy-MM-dd_HH:mm:ss_ffff");
        }

        private void btnDestroy_Click(object sender, EventArgs e)
        {
            destoryClick();
        }

        public void destoryClick()
        {
            try
            {
                consumer.Close();
                consumer.Dispose();
                producer.Close();
                producer.Dispose();
                session.Close();
                session.Dispose();
                connection.Stop();
                connection.Close();
                connection.Dispose();
            }
            catch (Exception e) { }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            //string txt = "Test message....";
            //rtxtMsgRcv.AppendText("Message Received " + timeStamp(DateTime.Now) + txt + "\n");
            //IConnectionFactory factory = new ConnectionFactory(connectUri);
            //IConnection connection = factory.CreateConnection("sempra.ops.gs.service", "sempra");
            //ISession session = connection.CreateSession();

            //destination = SessionUtil.GetDestination(session, "topic://sempra.ops.opsTracking.summary.update" + MAX_INACTIVITY_DURATION);
            //consumer = session.CreateConsumer(destination);
            //connection.Start();

            //consumer.Listener += new MessageListener(OnMessage);
//////////////////////////

            IConnectionFactory factory2 = new ConnectionFactory(connectUri);

            using (IConnection connection2 = factory2.CreateConnection())
            using (ISession session2 = connection2.CreateSession())
            {
                // Examples for getting a destination:
                //
                // Hard coded destinations:
                //    IDestination destination = session.GetQueue("FOO.BAR");
                //    Debug.Assert(destination is IQueue);
                //    IDestination destination = session.GetTopic("FOO.BAR");
                //    Debug.Assert(destination is ITopic);
                //
                // Embedded destination type in the name:
                //    IDestination destination = SessionUtil.GetDestination(session, "queue://FOO.BAR");
                //    Debug.Assert(destination is IQueue);
                //    IDestination destination = SessionUtil.GetDestination(session, "topic://FOO.BAR");
                //    Debug.Assert(destination is ITopic);
                //
                // Defaults to queue if type is not specified:
                //    IDestination destination = SessionUtil.GetDestination(session, "FOO.BAR");
                //    Debug.Assert(destination is IQueue);
                //
                // .NET 3.5 Supports Extension methods for a simplified syntax:
                //    IDestination destination = session.GetDestination("queue://FOO.BAR");
                //    Debug.Assert(destination is IQueue);
                //    IDestination destination = session.GetDestination("topic://FOO.BAR");
                //    Debug.Assert(destination is ITopic);

                IDestination destination2 = SessionUtil.GetDestination(session2, "topic://sempra.ops.opsTracking.summary.update" + MAX_INACTIVITY_DURATION);

                // Create a consumer and producer
                using (IMessageConsumer consumer2 = session2.CreateConsumer(destination2))
                using (IMessageProducer producer2 = session2.CreateProducer(destination2))
                {
                    // Start the connection so that messages will be processed.
                    connection2.Start();
                    //producer2.Persistent = true;
                    //producer2.RequestTimeout = receiveTimeout;
                    consumer2.Listener += new MessageListener(OnMessage);

                    // Send a message
                    ITextMessage request2 = session2.CreateTextMessage("Hello World!");
                    request2.NMSCorrelationID = "abc";
                    request2.Properties["NMSXGroupID"] = "cheese";
                    request2.Properties["myHeader"] = "Cheddar";

                    producer.Send(request2);

                    // Wait for the message
                    //semaphore.WaitOne((int)receiveTimeout.TotalMilliseconds, true);
                    //if (message == null)
                    //{
                    //    Console.WriteLine("No message received!");
                    //}
                    //else
                    //{
                    //    Console.WriteLine("Received message with ID:   " + message.NMSMessageId);
                    //    Console.WriteLine("Received message with text: " + message.Text);
                    //}
                }
            }

        }

        private void btnClearMsg_Click(object sender, EventArgs e)
        {
            rtxtMsgRcv.Clear();
        }

    }
}
