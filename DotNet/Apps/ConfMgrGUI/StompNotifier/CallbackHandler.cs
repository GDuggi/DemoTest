using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Collections;
using System.IO;
using System.Windows.Forms;
using Apache.NMS;
using Aff.Sif.MessageBusClient;
using OpsTrackingModel;

namespace OpsManagerNotifier
{
    public class CallbackHandler 
    {
        private string topicName;
        private NotifyCallBack callback;
        private Assembly assembly;
        private const string DATATYPE_NAMESPACE = "OpsTrackingModel.";
        private List<string> v_PermKeyList = new List<string>();
        private bool v_IsSuperUser = false;

        public CallbackHandler(NotifyCallBack callback, List<string> pPermKeyList, bool pIsSuperUser)
        {
            this.callback = callback;
            string appPath = Path.GetDirectoryName(Application.ExecutablePath);
            assembly  = Assembly.LoadFrom(appPath + @"\OpsManagerModel.dll");
            this.v_PermKeyList = pPermKeyList;
            this.v_IsSuperUser = pIsSuperUser;
        }

        public string TopicName
        {
            get { return topicName; }
            set { topicName = value; }
        }

        public void OnMessage(IMessage message)
        {
            object data = CreateDataObject(message);
            bool isPermKeyOk = false;

            //Israel 11/20/2015 -- Added support for permission keys.
            if (message.NMSType.ToUpper() == "SUMMARY")
            {
                SummaryData summaryData = data as SummaryData;
                isPermKeyOk = (String.IsNullOrEmpty(summaryData.PermissionKey)) || 
                              (v_PermKeyList.IndexOf(summaryData.PermissionKey) > -1);
                if (isPermKeyOk || v_IsSuperUser)
                    callback.Invoke(this, data);
            }
            //Israel 12/16/2015 -- Message publisher was escaping the : character-- this removes it.
            else if (message.NMSType.ToUpper() == "CONFIRM")
            {
                TradeRqmtConfirm confirmData = data as TradeRqmtConfirm;
                if (!String.IsNullOrEmpty(confirmData.XmitAddr))
                confirmData.XmitAddr = confirmData.XmitAddr.Replace(@"\", "");
                callback.Invoke(this, data);
            }
            else
            {
                callback.Invoke(this, data);
            }

            message.Acknowledge();
            //string testMessage = DateTime.Now.ToShortDateString() + ":" + DateTime.Now.ToLongTimeString() + ": " + message.ToString();
            //File.AppendAllText(@"_MessageDump.txt", testMessage + Environment.NewLine);
        }

        public void OnConnectionStatusChange(ConnectionStatusEvent statusEvent)
        {
            if (statusEvent.Status == ConnectionStatus.Connected)
            {
                // Notify the user in some way - update the status bar, have a "light" that toggles green or something.
            }
            else
            {
                // Notify the user in some way - update the status bar, have a "light" that toggles red or something.
            }
        }


        /* public void OnException(Exception e)
        {
            MessageBox.Show("Exception: " + "\n" + e.Message.ToString());
        } */

        //Necessary to use delegate because of "Cross-thread operation not valid" error
        //writing to form control.
        //delegate void OnMessageCallback(ITextMessage message);
        //public void OnMessage(IMessage message)
        //{
        //    // InvokeRequired required compares the thread ID of the
        //    // calling thread to the thread ID of the creating thread.
        //    // If these threads are different, it returns true.
        //    if (this.tboxReceivedMessage.InvokeRequired)
        //    {
        //        OnMessageCallback d = new OnMessageCallback(OnMessage);
        //        this.Invoke(d, new object[] { message });
        //    }
        //    else
        //    {
        //        try
        //        {
        //            ITextMessage msg = (ITextMessage)message;
        //            string txt = msg.Text;
        //            tboxReceivedMessage.AppendText("Message Received " + timeStamp(DateTime.Now) + ":  " + txt + "\n");
        //        }
        //        catch (Exception e)
        //        {
        //            MessageBox.Show("Exception receiving message: " + "\n" + e.Message.ToString());
        //        }
        //    }
        //}

        /*
        private object CreateDataObject(MapMessage message)
        {
            string propClassValue = message.GetString("class");
            string[] splitClassValue = propClassValue.Split(" ".ToCharArray());
            string className = splitClassValue[splitClassValue.Length - 1];
            Console.WriteLine("Class Name =" + className);
            Assembly assembly = Assembly.LoadFrom(@".\OpsManagerModel.dll");
            Type type = assembly.GetType(className);
            object dataObject = Activator.CreateInstance(type);
            PropertyInfo[] properties = dataObject.GetType().GetProperties();

            for (int i = 0; i < properties.Length; ++i)
            {
                PropertyInfo prop = properties[i];
                string mapMessageKeyName = "_" + prop.Name.ToLower();
                string mapMessageKeyValue = message.GetString(mapMessageKeyName);
                object propValue = GetMessageValue(prop, mapMessageKeyValue);
                if (propValue != null)
                {
                    prop.SetValue(dataObject,propValue,null);
                }
            }


            return dataObject;

        }
        */

        private object CreateDataObject(IMessage message) 
        {
            string propClassValue = message.Properties.GetString("class");
            string[] splitClassValue = propClassValue.Split(" ".ToCharArray());
            string className = splitClassValue[splitClassValue.Length - 1];
            //Israel 6/18/14 -- need to strip off package name from class
            int len = className.LastIndexOf(".");
            className = className.Remove(0, len + 1);
            className = DATATYPE_NAMESPACE + className;
            
            Type type = assembly.GetType(className);
            object dataObject = Activator.CreateInstance(type);
            PropertyInfo[] properties = dataObject.GetType().GetProperties();

            for (int i = 0; i < properties.Length; ++i)
            {
                PropertyInfo prop = properties[i];
                string mapMessageKeyName = prop.Name.ToLower();
                string mapMessageKeyValue = message.Properties.GetString(mapMessageKeyName);
                object propValue = GetMessageValue(prop, mapMessageKeyValue);
                if (propValue != null)
                {
                    prop.SetValue(dataObject, propValue, null);
                }
            }
            return dataObject;
        }

        private object GetMessageValue(PropertyInfo prop, string value)
        {
            object returnValue = null;
            if ( value == null || "".Equals(value))
            {
                return returnValue;
            }
            try
            {
                if (prop.PropertyType == typeof(System.DateTime))
                {
                    //Israel 3/24/15 -- Stomp returning dates with value: "01/30/2015 00\\:00\\:00"
                    value = value.Replace("\\", "");
                    returnValue = System.DateTime.ParseExact(value, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                }
                else if (prop.PropertyType == typeof(System.Single))
                {
                    returnValue = System.Single.Parse(value);
                }
                else if (prop.PropertyType == typeof(System.Int32))
                {
                    returnValue = System.Int32.Parse(value);
                }
                else if (prop.PropertyType == typeof(System.Int64))
                {
                    returnValue = System.Int64.Parse(value);
                }
                else if (prop.PropertyType == typeof(System.Double))
                {
                    returnValue = System.Double.Parse(value);
                }
                else if (prop.PropertyType == typeof(System.String))
                {
                    returnValue = value;
                }
            }
            catch (Exception e)
            {
               // Console.WriteLine("Error converting " + e.Message);
            }
            return returnValue;
        }
    }
}
