using System;
using System.Collections.Generic;
using System.Text;

namespace OpsManagerNotifier
{
    public delegate void NotifyCallBack(object sender, object data);
    public class ListenerManager
    {
        private const string PROJ_FILE_NAME = "ListenerManager";
        private Dictionary<string, NotifyCallBack> notifier = new Dictionary<string, NotifyCallBack>();
        private Dictionary<string, string> notifierFilter = new Dictionary<string, string>();
        private string updMsgServerName;
        private string updMsgUserId;
        private string updMsgPwd;
        private List<Listener> listenerList = new List<Listener>();
        private List<string> v_PermKeyList = new List<string>();
        private bool v_IsSuperUser = false;
        

        public void AddListner(string subscriberTopicName, NotifyCallBack callback,string filterCondition)
        {
           // notifier.Add(subscriberTopicName,callback);
            notifier[subscriberTopicName] = callback;
            //notifierFilter.Add(subscriberTopicName, filterCondition);
            notifierFilter[subscriberTopicName] = filterCondition;
        }

        public ListenerManager(string pUpdMsgServer, string pUpdMsgUser, string pUpdMsgPwd, List<string> pPermKeyList, bool pIsSuperUser)
        {
            this.updMsgServerName = pUpdMsgServer;
            this.updMsgUserId = pUpdMsgUser;
            this.updMsgPwd = pUpdMsgPwd;
            this.v_PermKeyList = pPermKeyList;
            this.v_IsSuperUser = pIsSuperUser;
        }
        public void StartListener()
        {
            if (notifier.Count <= 0)
            {
                throw new Exception("No topic name added." + Environment.NewLine +
                    "Error CNF-368 in " + PROJ_FILE_NAME + ".StartListener().");
            }
            foreach(string key in notifier.Keys)
            {
                Listener listener = new Listener(this.updMsgServerName, this.updMsgUserId, this.updMsgPwd,key, notifier[key], 
                    notifierFilter[key], v_PermKeyList, v_IsSuperUser);
                listener.Start();
                listenerList.Add(listener);
            }
        }
        public void StopListener()
        {
            foreach (Listener listener in listenerList)
            {
                listener.Stop();
            }

        }
    }
}
