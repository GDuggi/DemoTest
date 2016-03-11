using System;
using System.Collections.Generic;
using System.Text;
using Apache.NMS;

namespace OpsManagerNotifier
{
    public class JMSFactory
    {
        private Dictionary<string,ISubscriber> subscriberList = new Dictionary<string,ISubscriber>();
        private static JMSFactory factory = null;

        private string tibcoServerName;
        private string tibcoUser;
        private string tibcoPwd;

        private JMSFactory(string serverName,string userId,string pwd)
        {
            this.tibcoServerName = serverName;
            this.tibcoUser = userId;
            this.tibcoPwd = pwd;
        }
        public ISubscriber GetInstance(string topicName,CallbackHandler handler,string filterCondition)
        {
            ISubscriber subscriber = null;
            if (subscriberList.ContainsKey(topicName))
            {
                subscriber = subscriberList[topicName];
            }
            else
            {
                subscriber = new JMSSubscriber(tibcoServerName, tibcoUser, tibcoPwd, topicName, handler, filterCondition);
            }
            return subscriber;
        }

        public static JMSFactory getFactory(string server, string userId, string pwd)
        {

            if (factory == null)
            {
                factory = new JMSFactory(server, userId, pwd);
            }
            return factory;
        }

    }
}
