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
    public class TopicSubscriber
    {
        private bool disposed = false;
        private readonly ISession session;
        private readonly ITopic topic;
        private readonly string destination;

        public event MessageListener OnMessageReceived;
        public IMessageConsumer Consumer { get; private set; }
        public string ConsumerId { get; private set; }

        public TopicSubscriber(ISession session, string destination)
        {
            this.session = session;
            this.destination = destination;
            topic = SessionUtil.GetTopic(session, this.destination);
        }

        public void Start(string consumerId)
        {
            ConsumerId = consumerId;
            Consumer = session.CreateDurableConsumer(topic, consumerId, null, false);
            Consumer.Listener += (message =>
                {
                    var textMessage = message as ITextMessage;
                    if (textMessage == null) throw new InvalidCastException();
                    if (OnMessageReceived != null)
                    {
                        OnMessageReceived(message);
                    }
                });
        }

        public void Dispose()
        {
            if (disposed) return;
            if (Consumer != null)
            {
                Consumer.Close();
                Consumer.Dispose();
            }
            disposed = true;
        }
    }
}
