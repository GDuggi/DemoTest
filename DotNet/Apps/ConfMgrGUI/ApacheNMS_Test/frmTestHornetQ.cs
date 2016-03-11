using Apache.NMS;
using Apache.NMS.Stomp;
using Apache.NMS.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ApacheNMS_Test
{
    public partial class frmTestHornetQ : Form
    {
        private const string TOPIC_PREFIX = @"topic://";
        private const string QUEUE_PREFIX = @"queue://";
        //Measured in milliseconds. 30000 = default, 0=Unlimited
        private const string MAX_INACTIVITY_DURATION = "?wireFormat.maxInactivityDuration=0";
        private const string TOPIC_NAME = "sempra.ops.opsTracking.summary.update";
        private const string CLIENT_ID = "ifrankel.test.clientId";
        private const string CONSUMER_ID = "ifrankel.test.subscriber";

        private IConnectionFactory _connectionFactory;
        private IConnection _connection;
        private ISession _session;
        //private IDestination destination;
        private IMessageConsumer _consumer;
        private IMessageProducer _producer;
        private Uri _connectUri;
        private TopicSubscriber _topicSubscriber;

        public frmTestHornetQ()
        {
            InitializeComponent();

            cboxDestinationType.SelectedIndex = Properties.Settings.Default.DefaultDestination.ToUpper().Equals("QUEUE") ? 0 : 1;
            foreach (var value in Properties.Settings.Default.QueueList)
                cboxQueueList.Items.Add(value);
            foreach (var value in Properties.Settings.Default.TopicList)
                cboxTopicList.Items.Add(value);

            tstripMessageServerUri.Text = Properties.Settings.Default.MessageServerUri;
            tstripUserId.Text = Properties.Settings.Default.MessageServerUserId;
            tstripPassword.Text = Properties.Settings.Default.MessageServerPassword;
            cboxQueueList.SelectedIndex = 0;
            cboxTopicList.SelectedIndex = 0;
            tboxSimpleMessage.Text = Properties.Settings.Default.DefaultSimpleMessage;
            _connectUri = new Uri(Properties.Settings.Default.MessageServerUri);
        }

                //IDestination destination;
                //        string destinationTarget;
                //switch (cboxDestinationType.SelectedIndex)
                //{
                //    case 0:
                //        destinationTarget = QUEUE_PREFIX + cboxQueueList.Items[cboxQueueList.SelectedIndex] + MAX_INACTIVITY_DURATION;
                //        break;
                //    case 1:
                //        destinationTarget = TOPIC_PREFIX + cboxTopicList.Items[cboxTopicList.SelectedIndex] + MAX_INACTIVITY_DURATION;
                //        break;
                //    default:
                //        throw new Exception("Please select a destination.");
                //}
                //destination = SessionUtil.GetDestination(_session, destinationTarget);

        private void btnSetup_Click(object sender, EventArgs e)
        {
            try
            {
                //TearDown();
                //_connectionFactory = new ConnectionFactory(_connectUri, CLIENT_ID);               
                //_connection = _connectionFactory.CreateConnection(Properties.Settings.Default.MessageServerUserId, Properties.Settings.Default.MessageServerPassword);

                Environment.SetEnvironmentVariable("is.hornetQ.client", "true");
                _connection = CreateConnection();
                _connection.ExceptionListener += new ExceptionListener(OnException);
                _session = _connection.CreateSession(AcknowledgementMode.AutoAcknowledge);

                string destName = cboxTopicList.Text;
                IDestination dest = SessionUtil.GetDestination(_session, TOPIC_PREFIX + destName);
                var consumer = _session.CreateConsumer(dest);
                consumer.Listener += new MessageListener(OnMessage);

                ITextMessage request = _session.CreateTextMessage("Connection verification has been received.");
                
                //request.NMSCorrelationID = "abc";
                //request.Properties["NMSXGroupID"] = "cheese";
                //request.Properties["myHeader"] = "Cheddar";
                using (IMessageProducer producer = _session.CreateProducer(dest))
                {
                    producer.Send(request);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception doing setup: " + "\n" + ex.Message.ToString());
            }
        }

        private static IConnection CreateConnection()
        {
            IConnectionFactory factory = new ConnectionFactory(Properties.Settings.Default.MessageServerUri);
            IConnection connection = factory.CreateConnection(Properties.Settings.Default.MessageServerUserId, Properties.Settings.Default.MessageServerPassword);
            connection.Start();
            return connection;
        }

        public void TearDown()
        {
            try
            {
                Thread.Sleep(1000);
                _topicSubscriber.Dispose();
                _session.Close();
                _session.Dispose();

                _connection.Stop();
                _connection.Close();
                _connection.Dispose();
            }
            catch (Exception ex)
            { }
        }

        private void btnPublish_Click(object sender, EventArgs e)
        {
            try
            {
                using (var publisher = new TopicPublisher(_session, TOPIC_PREFIX + TOPIC_NAME))
                {
                    publisher.SendMessage(tboxSimpleMessage.Text);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception sending message: " + "\n" + ex.Message.ToString());
            }
        }

        public static String timeStamp(DateTime value)
        {
            return value.ToString("yyyy-MM-dd_HH:mm:ss_ffff");
        }

        public void destroyObjects()
        {
            try
            {
                _consumer.Close();
                _consumer.Dispose();
                _producer.Close();
                _producer.Dispose();
                _session.Close();
                _session.Dispose();
                _connection.Stop();
                _connection.Close();
                _connection.Dispose();
            }
            catch (Exception e) { }
        }

        //Necessary to use delegate because of "Cross-thread operation not valid" error
        //writing to form control.
        delegate void OnMessageCallback(ITextMessage message);
        public void OnMessage(IMessage message)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.tboxReceivedMessage.InvokeRequired)
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
                    tboxReceivedMessage.AppendText("Message Received " + timeStamp(DateTime.Now) + ":  " + txt + "\n");
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

        private void cboxDestinationType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboxDestinationType.SelectedIndex.Equals(0))
            {
                cboxQueueList.Enabled = true;
                cboxTopicList.Enabled = false;
            }
            else
            {
                cboxTopicList.Enabled = true;
                cboxQueueList.Enabled = false;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            destroyObjects();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //String testDateStr = "01/30/2015 00:00:00";
            //object testDateObj = null;
            //testDateObj = System.DateTime.ParseExact(testDateStr, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            //MessageBox.Show("Test DateAsString=" + testDateObj.ToString());


            String testStr = "01/30/2015 00\\:00\\:00";
            testStr = testStr.Replace("\\", "");
            MessageBox.Show("testStr=" + testStr);
        }


    }
}
