using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apache.NMS;
//using Apache.NMS.Stomp;
using Apache.NMS.Util;

namespace ApacheNMS_Test
{
    public class TopicPublisher : IDisposable
    {
        private bool disposed;
        private readonly ISession session;
        private readonly ITopic topic;

        public TopicPublisher(ISession session, string topicName)
        {
            this.session = session;
            DestinationName = topicName;
            topic = SessionUtil.GetTopic(session, DestinationName);
            Producer = session.CreateProducer(topic);
        }

        public IMessageProducer Producer { get; private set; }
        public string DestinationName { get; private set; }

        public void SendMessage(string message)
        {
            if (disposed)
                throw new ObjectDisposedException(GetType().Name);
            var textMessage = Producer.CreateTextMessage(message);
            Producer.Send(textMessage);
        }

        public void Dispose()
        {
            if (disposed) return;
            Producer.Close();
            Producer.Dispose();
            disposed = true;
        }
    }
}
