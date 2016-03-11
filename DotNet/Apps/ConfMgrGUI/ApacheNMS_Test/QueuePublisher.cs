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
    public class QueuePublisher
    {
        private bool disposed;
        private readonly ISession session;
        private readonly IQueue queue;

        public IMessageProducer Producer { get; private set; }
        public string DestinationName { get; private set; }

        public QueuePublisher(ISession session, string queueName)
        {
            this.session = session;
            DestinationName = queueName;
            queue = SessionUtil.GetQueue(session, DestinationName);
            Producer = session.CreateProducer(queue);
        }

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
