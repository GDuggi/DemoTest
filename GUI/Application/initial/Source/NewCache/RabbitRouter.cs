using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using com.amphora.entities.router;
using com.amphora.events.listener;
//using Newtonsoft.Json;
using NSRMLogging;
using RabbitMQ.Client;

namespace NSRiskManager {
    public class RabbitRouter : IRabbitRouter,IPortfolioRabbitRouterActions {

        #region constants
        const string DEFAULT_USERID = "mramos";
        const string PORT_EXCHANGE = "portfolio-exchange";
        const string PORT_ROUTING_KEY = "portfolio-exchange-route";
        #endregion

        #region fields
        public static RabbitRouter shared = new RabbitRouter();
        readonly Dictionary<string,TopicThreadListener> subData = new Dictionary<string,TopicThreadListener>();
        readonly IDictionary<string,Guid> guidMap = new Dictionary<string,Guid>();
        int requestId = 0;
        #endregion

        #region cctor
        static RabbitRouter() {
            shared.userid = DEFAULT_USERID;
        }

        #endregion

        #region ctor
        RabbitRouter() { }
        #endregion

        #region IRabbitRouter implementation

        public void subscribe(IRabbitListener irl,java.util.List desiredTopics,int requestId) {
            List<string> topics;
            TopicThreadListener tsd;
            Guid guid;

            Util.show(MethodBase.GetCurrentMethod(),"[Request# " + requestId+"] starts");
            Trace.IndentLevel++;
            if ((topics = extractTopics(desiredTopics)).Count > 0)
                foreach (string aTopic in topics) {
                    if (!subData.ContainsKey(aTopic)) {
                        subData.Add(
                            aTopic,
                            tsd = new TopicThreadListener(aTopic,new ForwardRabbitMsg(doForward),irl));
                        guidMap.Add(aTopic,guid = Guid.NewGuid());
                        tsd.start(mcc);
                    }
                    subData[aTopic].useCount++;
                }
            Trace.IndentLevel--;
            Util.show(MethodBase.GetCurrentMethod(),"[Request# " + requestId + "] ends");
        }

        List<string> extractTopics(java.util.List desiredTopics) {
            List<string> topics = new List<string>();
            string aTopic0;
            int n;

            if ((n = desiredTopics.size()) > 0) {
                for (int i = 0 ;i < n ;i++) {
                    aTopic0 = desiredTopics.get(i) as string;
                    if (!topics.Contains(aTopic0))
                        topics.Add(aTopic0);
                }
                if ((n = topics.Count) > 0) {
                    if (n > 1)
                        topics.Sort();
                    showTopics(requestId,topics);
                }
            }
            return topics;
        }

        static void showTopics(int requestId,List<string> topics) {
            StringBuilder sb;
            int n = topics == null ? 0 : topics.Count;

            sb = new StringBuilder();
            sb.Append("[request #" + requestId + "] subscribe to topics ");
            for (int i = 0 ;i < n ;i++) {
                if (i > 0)
                    sb.Append(", ");
                sb.Append(topics[i]);
            }
      
            Debug.WriteLine(sb.ToString());
        }

        public void unsubscribe(IRabbitListener irl,java.util.List topicsForRemoval,int requestId) {
            Util.show(MethodBase.GetCurrentMethod(),"[Request# " + requestId+"] starts");
            Trace.IndentLevel++;
            if (topicsForRemoval.size() > 0)
                unsubscribeFrom(makeUniqueList(topicsForRemoval));
            Trace.IndentLevel--;
            Util.show(MethodBase.GetCurrentMethod(),"[Request# " + requestId + "] ends");
        }

        void unsubscribeFrom(List<string> tmp) {
            List<string> topicsToRemove;
            int n;

            if ((n = tmp.Count) > 0) {
                topicsToRemove = new List<string>();
                if (n > 1)
                    tmp.Sort();
                unsubscribeFromTopics(tmp,topicsToRemove);
                removeTopicData(topicsToRemove);
            }
        }

        void removeTopicData(List<string> topicsToRemove) {
            if (topicsToRemove.Count > 0) {
                foreach (string aTopic3 in topicsToRemove) {
                    if (subData.ContainsKey(aTopic3))
                        subData.Remove(aTopic3);
                    if (guidMap.ContainsKey(aTopic3))
                        guidMap.Remove(aTopic3);
                }
            }
        }

        void unsubscribeFromTopics(List<string> tmp,List<string> topicsToRemove) {
            foreach (string aTopic2 in tmp) {
                if (subData.ContainsKey(aTopic2)) {
                    subData[aTopic2].useCount--;
                    if (subData[aTopic2].useCount < 1) {
                        subData[aTopic2].shutdown(mcc);
                        if (!topicsToRemove.Contains(aTopic2))
                            topicsToRemove.Add(aTopic2);
                    }
                }
            }
        }

        static List<string> makeUniqueList(java.util.List listOfStrings) {
            List<string> tmp = new List<string>();
            string aTopic;
            int n;

            if ((n = listOfStrings.size()) > 0)
                for (int i2 = 0 ;i2 < n ;i2++)
                    if (!tmp.Contains(aTopic = listOfStrings.get(i2) as string))
                        tmp.Add(aTopic);
            return tmp;
        }
        #endregion

        #region properties
        public string userid { get; set; }
        //     [Obsolete("remove this",true)]
        static MyChannelContainer mcc { get; set; }
        #endregion

        /*
        public static void blah(int[] portNums) {
            shared.requestPortfolioTopic(mcc,portNums);
        }
         * */

        void doForward(object sender,IRabbitListener irl,string topidId,object receivedMsg) {
          
            irl.objectReceived(receivedMsg,topidId);

            if (string.Compare(topidId,"system.admin.message.exchange",true) != 0)
                Util.show(MethodBase.GetCurrentMethod(),topidId + ":" + receivedMsg.ToString());
        }

    
        string[] convertToStringVector(int[] portNums) {
            List<string> ret = new List<string>();

            Util.show(MethodBase.GetCurrentMethod());
            if (portNums != null)
                foreach (int anInt in portNums)
                    ret.Add(anInt.ToString());
            if (ret.Count > 0)
                ret.Sort();
            return ret.ToArray();
        }

       

        internal void startup() 
        {
            if (mcc == null)

                mcc = new MyChannelContainer(ChannelUtil.createContainer());

        }

        internal void shutdown() {
            List<string> allKeys;

            if (subData.Count > 0) {
                allKeys = new List<string>();
                foreach (string akey in subData.Keys)
                    if (!allKeys.Contains(akey))
                        allKeys.Add(akey);
                unsubscribeFrom(allKeys);
            }
        }

        internal void subscribeTo(IRabbitListener listener,string[] topics) {
            if (topics != null)
                subscribe(listener,makeList(topics),++requestId);
        }

        java.util.List makeList(string[] topics) {
            java.util.ArrayList ret = new java.util.ArrayList();

            if (topics != null)
                foreach (string aTopic in topics)
                    ret.add(aTopic);
            return ret;
        }

        #region IPortfolioRabbitRouterActions
        void IPortfolioRabbitRouterActions.intercept(com.amphora.pojo.PortfolioCRUDRequest pcrudr,java.util.Map m) {
            //          Util.show(MethodBase.GetCurrentMethod());
            Util.show(MethodBase.GetCurrentMethod(),"have REFKEY=" + pcrudr.getRefKey());
        }

        
        void IPortfolioRabbitRouterActions.transactedSend(string json,java.util.Map headers,string contentType,java.lang.Integer deliveryMode0) {
            IBasicProperties pro;
            IDictionary<string,object> headerDict = convert1(headers);
           
            string quet;

            lock (this) 
            {
                lock (mcc.channelLock) 
                {
                    quet = mcc.channel.QueueDeclare("aqueue",true,false,false,null);
                    pro = mcc.channel.CreateBasicProperties();
                }
            }
            if (headerDict.Count > 0)
                pro.Headers = headerDict;
            pro.DeliveryMode = 2; // non-persistent
            pro.ContentType = contentType;
            pro.ReplyTo = quet;
            Debug.Print("using to '" + TopicThreadListener.ROUTING_KEY_CRUD + "' with payload '" + json + "'.");
            lock (mcc.channelLock) 
            {
                try 
                {
                    mcc.channel.BasicPublish(
                        string.Empty,
                        TopicThreadListener.ROUTING_KEY_CRUD, // portfolio.crud
                        pro,
                        mcc.encoding.GetBytes(json));
                } 
                catch (Exception ex) 
                {
                    Util.show(MethodBase.GetCurrentMethod(),ex);
                }
            }
            
        }
         

        IDictionary<string,object> convert1(java.util.Map aMap) {
            IDictionary<string,object> ret = new Dictionary<string,object>();
            string key;

            if (aMap.size() > 0) {
                var avar5 = aMap.keySet().iterator();
                while (avar5.hasNext())
                    ret.Add(key = avar5.next().ToString(),aMap.get(key).ToString());
            }
            return ret;
        }
        #endregion IPortfolioRabbitRouterActions
    }
    delegate void ForwardRabbitMsg(object sender,IRabbitListener irl,string topidId,object receivedMsg);
}