using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Apache.NMS;
using Apache.NMS.Stomp;
using Apache.NMS.Util;
using Aff.Sif.MessageBusClient;

namespace OpsManagerNotifier
{
    class JMSSubscriber : ISubscriber
    {
        private const string TOPIC_PREFIX = @"topic://";
        private string topicName;
        private CallbackHandler handler;
        private string filterCondition;
        private bool isSubscribing = false;

        private string jmsServer;
        private string userId;
        private string password;

        IConnectionFactory factory;
        IConnection connection;
        ISession session;
        ITopic topic;
        IMessageConsumer consumer;

        public JMSSubscriber(string server,string userId, string password, string topicName,CallbackHandler handler,string filterCondition)
        {
            this.jmsServer = server;
            this.userId = userId;
            this.password = password;
            this.topicName = topicName;
            this.handler = handler;
            this.filterCondition = filterCondition;
        }
        
        public void Subscribe()
        {
            if (isSubscribing == false)
            {
                //Israel - Replaced 5/14/15 to resolve message timeout issue.
                //Israel 12/3/14
                /* Environment.SetEnvironmentVariable("is.hornetQ.client", "true");
                connection = CreateConnection();
                connection.ExceptionListener += new ExceptionListener(handler.OnException);
                session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge);
                IDestination dest = SessionUtil.GetDestination(session, TOPIC_PREFIX + this.topicName);
                var consumer = session.CreateConsumer(dest);
                consumer.Listener += new MessageListener(handler.OnMessage);
                this.isSubscribing = true; */

                //Israel 5/14/15 code supplied by Harley to resolve message timeout issue.
                //optionally set up tracing, uncomment below or drive this off a configuration property.
                //MessageBusFactory.Instance.IsMessageLoggingEnabled = true;
                //string server = "AFF01INF01:61613";
                IMessageBus bus = MessageBusFactory.Instance.GetMessageBus(jmsServer, userId, password); // only have to pass server:port here i.e. AFF01INF01:61613
                bus.OnConnectionStatusChange += handler.OnConnectionStatusChange;
                bus.Subscribe(topicName, handler.OnMessage, MessageBusDestinationType.Topic);
                this.isSubscribing = true;
            }
        }

        public void UnSubscribe()
        {
            //Israel 5/14/15 -- Removed to implement Harley's message timeout fix.
            /* if (isSubscribing == true)
            {
                connection.Stop();
                isSubscribing = false;
            } */

            IMessageBus bus = MessageBusFactory.Instance.GetMessageBus(jmsServer, userId, password);
            bus.OnConnectionStatusChange -= handler.OnConnectionStatusChange;
            bus.UnSubscribe(topicName, handler.OnMessage);
        }

        private IConnection CreateConnection()
        {
            IConnectionFactory factory = new ConnectionFactory(jmsServer);
            IConnection connection = factory.CreateConnection(userId, password);
            connection.Start();
            return connection;
        }

        private IConnectionFactory GetConnectionFactory()
        {
            IConnectionFactory factory = new ConnectionFactory(jmsServer);
            return factory;
        }
        
        // Israel 5/14/15 -- Deprecated
        /* public void SetFilterCondition(string messageFilter)
        {
            if (isSubscribing == true) {
                this.filterCondition = messageFilter;
                connection.Stop();
                consumer = session.CreateConsumer(topic, filterCondition);
                consumer.Listener += new MessageListener(handler.OnMessage);
                connection.Start();
            }
        } */

    }
}
