using System;
using System.Collections.Generic;
using System.Text;

namespace OpsManagerNotifier
{
    [Serializable]
    class OpsManagerMessage 
    {
        private string messageType;
        private DateTime publishedDateTime;
        private object data;


        public DateTime PublishedDateTime
        {
            get { return publishedDateTime; }
            set { publishedDateTime = value; }
        }

        public object Data
        {
            get { return data; }
            set { data = value; }
        }

        
        public string MessageType
        {
            get { return messageType; }
            set { messageType = value; }
        }
    }
}
