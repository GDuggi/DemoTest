using System;
using System.Collections.Generic;
using System.Text;

namespace OpsManagerNotifier
{
    class Listener
    {
        private string topicName;
        private NotifyCallBack callback;
        private ISubscriber subscriber;
        private CallbackHandler handler;
        private string filterCondition;
        public Listener(string server, string userId, string pwd, string topicName, NotifyCallBack callback,
            string filterCondition, List<string> pPermKeyList, bool pIsSuperUser)
        {
            this.topicName = topicName;
            this.callback = callback;
            handler = new CallbackHandler(callback, pPermKeyList, pIsSuperUser);
            handler.TopicName = this.topicName;
            JMSFactory factory = JMSFactory.getFactory(server, userId, pwd);
            subscriber = factory.GetInstance(topicName,handler,filterCondition);
            this.filterCondition = filterCondition;
        }

        public void Start()
        {
            subscriber.Subscribe();
        }
        public void Stop()
        {
            subscriber.UnSubscribe();
        }
    }
}
