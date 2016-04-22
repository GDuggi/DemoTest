using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using com.amphora.cayenne.entity;
using Newtonsoft.Json;
using NSRMCommon;
using NSRMLogging;
using RabbitMQ.Client;

namespace NSRiskManager {
    class NewPortGenerator : IDisposable {
        #region constants
        public const string CRUDE_TYPE_ID = "com.amphora.pojo.PortfolioCRUDRequest";
        const string ROUTING_KEY_CRUD = "portfolio.crud";
        #endregion

        #region fields
        public static NewPortGenerator shared = new NewPortGenerator();
        ConnectionFactory factory;
        IConnection connection;
        ChannelContainer cc;
        string lastResponse;
        Thread listeningThread;
        readonly object dataLock = new object();
        #endregion

        #region ctor
        NewPortGenerator() {
            try {
                if (factory == null)
                    factory = new ConnectionFactory {
                        HostName = "172.16.143.199",
                        Port = 5672,
                        UserName = "guest",
                        Password = "guest"
                    };
                if (connection == null)
                    connection = factory.CreateConnection();
                if (cc == null) {
                    cc = createContainer(connection,Guid.NewGuid().ToString());
                }
            } catch (Exception ex) {
                Util.show(MethodBase.GetCurrentMethod(),ex);
            }
        }

        #endregion

        #region properties
        public bool disposed { get; private set; }
        public PortfolioCRUDEvent receivedEvent { get; private set; }
        public static bool isConnected { get; private set; }
        #endregion

        #region IDisposable implementation
        void IDisposable.Dispose() {
            Util.show(MethodBase.GetCurrentMethod());
            if (!disposed) {
                disposed = true;
            }
        }
        #endregion

        #region container methods
        internal static ChannelContainer createContainer() {
            return createContainer(shared.connection,Guid.NewGuid().ToString());
        }

        internal static ChannelContainer createContainer(string aReplyId) {
            return createContainer(shared.connection,aReplyId);
        }

        internal static ChannelContainer createContainer(IConnection connection,string aReplyId) {
            ChannelContainer cc = new ChannelContainer(aReplyId);

            cc.setup(connection);
            return cc;
        }
        #endregion

        #region startup/shutdown methods

        internal static void shutdown() {
            shared.shutdown2();
        }

        void shutdown2() {
            stopListening(cc);
        }

        internal void stopListening(ChannelContainer cc) {
            Util.show(MethodBase.GetCurrentMethod(),"signaling thread.");
            cc.threadLock.Set();
            cc.threadLock.WaitOne();
            Util.show(MethodBase.GetCurrentMethod(),"thread-lock signalled");
            if (listeningThread != null) {
                listeningThread.Join();
                Util.show(MethodBase.GetCurrentMethod(),"listening-thread done.");
                cc.shutdown();
                listeningThread = null;
            } else
                Debug.Print("no thread.");
            if (this.connection != null) {
                connection.Close(0,"OK");
                connection.Dispose();
                connection = null;
            }
            if (connection != null) {
                try {
                    connection.Close(0,"OK");
                } catch (Exception ex) {
                    Util.show(MethodBase.GetCurrentMethod(),ex);
                }
                connection.Dispose();
                connection = null;
            }
            if (factory != null)
                factory = null;
        }
        #endregion

        #region CRUD methods
        internal static bool updatePortfolio(Portfolio aport,int parentPortNum,bool isLink,List<PortfolioTag> tags) {
            return shared.localUpdatePortfolio(aport,parentPortNum,isLink,tags);
        }
        internal static bool deletePortfolio(int aPortNum) {
            return shared.deleteLocalPort(aPortNum);
        }

        internal bool createNewPortfolio(Portfolio aport,int parentPortNum,List<PortfolioTag> tags) {
            PortfolioCRUDRequest request;
            IBasicProperties props;
            MyPortDTO portFolioDto;
            bool ret = false;
            int nret = 0;

            portFolioDto = populateDTO(aport,out props,parentPortNum,false,tags);
            request = createPortfolioCRUDRequest(portFolioDto,"CREATE");
            cc.dataLock.Reset();
            sendPortfolioCRUDRequest(cc,request,props);

            bool found = false;
            while (!found) {
                if (cc.dataLock.WaitOne(100)) {
                    found = true;
                    ret = true;
                    cc.dataLock.Reset();
                } else {
                    nret++;
                    if (nret > 100) {
                        lock (dataLock) {
                            if (!string.IsNullOrEmpty(lastResponse)) {
                                Util.show("LastResponse=\r\n" + lastResponse);
                                lastResponse = null;
                            }
                        }
                        ret = false;
                        break;
                    }
                    Thread.Sleep(100);
                }
            }
            return ret;
        }
        bool localUpdatePortfolio(Portfolio aport,int parentPortNum,bool isLink,List<PortfolioTag> tags) {
            PortfolioCRUDRequest request;
            IBasicProperties props;
            MyPortDTO portFolioDto;
            bool ret = false;
            int nret = 0;

            portFolioDto = populateDTO(aport,out props,parentPortNum,isLink,tags);
            request = createPortfolioCRUDRequest(portFolioDto,"UPDATE");
            cc.dataLock.Reset();
            sendPortfolioCRUDRequest(cc,request,props);

            bool found = false;
            while (!found) {
                if (cc.dataLock.WaitOne(100)) {
                    found = true;
                    ret = true;
                    cc.dataLock.Reset();
                } else {
                    nret++;
                    if (nret > 100) {
                        lock (dataLock) {
                            if (!string.IsNullOrEmpty(lastResponse)) {
                                Util.show("LastResponse=\r\n" + lastResponse);
                                lastResponse = null;
                            }
                        }
                        ret = false;
                        break;
                    }
                    Thread.Sleep(100);
                }
            }
            return ret;
        }
        bool deleteLocalPort(int aPortNum) {
            PortfolioCRUDRequest request;
            IBasicProperties props;
            MyPortDTO portFolioDto;
            bool ret = false;
            int nret = 0;

            portFolioDto = createPort(false);
            props = createProps(cc,CRUDE_TYPE_ID);
            portFolioDto.portNum = aPortNum;
            request = createPortfolioCRUDRequest(portFolioDto,"DELETE");
            cc.dataLock.Reset();
            sendPortfolioCRUDRequest(cc,request,props);

            bool found = false;
            while (!found) {
                if (cc.dataLock.WaitOne(100)) {
                    found = true;
                    ret = true;
                    cc.dataLock.Reset();
                } else {
                    nret++;
                    if (nret > 100) {
                        lock (dataLock) {
                            if (!string.IsNullOrEmpty(lastResponse)) {
                                Util.show("LastResponse=\r\n" + lastResponse);
                                lastResponse = null;
                            }
                        }
                        ret = false;
                        break;
                    }
                    Thread.Sleep(100);
                }
            }
            return ret;
        }
        #endregion CRUD methods

        #region object-creation methods
        static MyPortDTO createPort(bool full) {
            MyPortDTO portFolioDto = new MyPortDTO();

            if (full) {
                portFolioDto.desiredPlCurrCode = "USD";
                portFolioDto.linkInd = "Y";
                portFolioDto.parentPortId = 1;
                portFolioDto.portClass = "T";
                portFolioDto.portFullName = "Strategy";
                portFolioDto.portShortName = "Socal Strat";
                portFolioDto.portType = "R";
            }
            return portFolioDto;

        }
        static IBasicProperties createProps(ChannelContainer cc,string receivingTypeId) {
            IBasicProperties props;
            IDictionary<string,object> otherHeader;

            if (cc == null)
                throw new ArgumentNullException("cc",typeof(ChannelContainer).FullName + " is null!");
            if (cc.channel == null)
                throw new ArgumentNullException("cc.channel",typeof(IModel).FullName + " is null!");
            if (cc.channelLock == null)
                throw new ArgumentNullException("cc.channelLock","channel-lock is null!");
            otherHeader = new Dictionary<string,object>();
            otherHeader.Add("__TypeId__",receivingTypeId);
            lock (cc.channelLock)
                props = cc.channel.CreateBasicProperties();
            props.ContentType = Util.CONTENT_TYPE;
            props.DeliveryMode = 2;
            props.Headers = otherHeader;
            return props;
        }

        public static Portfolio createDefaultPortfolio() {
            Portfolio ret = new Portfolio();

            ret.setPortShortName("New portfolio");
            ret.setPortFullName(ret.getPortShortName());
            ret.setNumHistoryDays(new java.lang.Integer(0));
            ret.setDesiredPlCurrCode("USD");
            ret.setPortType("R");
            ret.setPortLocked(new java.lang.Short(0));
            ret.setPortRefKey(string.Empty);
            addRequiredPortfolioTags(ret);
            return ret;
        }

        public static void addRequiredPortfolioTags(Portfolio newCopy) {
            foreach (PortfolioTag pt in readPortfolioTagDefs())
                newCopy.addPortfolioTag(pt);
        }

        public static List<WrappedPortTag> readWrappedPortfolioTags() {
            return readWrappedPortfolioTags(true);
        }

        public static List<WrappedPortTag> readWrappedPortfolioTags(bool requiredOnly) {
            return wrap(readPortfolioTagDefs(requiredOnly));
        }

        static List<WrappedPortTag> wrap(List<PortfolioTag> list) {
            List<WrappedPortTag> ret = new List<WrappedPortTag>();

            if (list != null)
                foreach (PortfolioTag pt in list)
                    ret.Add(new WrappedPortTag(pt));
            return ret;
        }

        static List<PortfolioTag> readPortfolioTagDefs() {
            return readPortfolioTagDefs(true);
        }

        static List<PortfolioTag> readPortfolioTagDefs(bool requiredOnly) {
            List<PortfolioTag> ret = new List<PortfolioTag>();
            PortfolioTag pt;

            foreach (var avar2 in SharedContext.readPortTagDefs()) {
                pt = new PortfolioTag();
                pt.setTagName(avar2.tagName);
                ret.Add(pt);
            }
            return ret;
        }

        public static Portfolio portfolioData(out List<PortfolioTag> portTags) {
            Portfolio ret = createDefaultPortfolio();
            java.util.List tagList;
            int n;

            portTags = new List<PortfolioTag>();
            tagList = ret.localTagList();
            if ((n = tagList.size()) > 0)
                for (int i = 0 ;i < n ;i++)
                    portTags.Add(tagList.get(i) as PortfolioTag);
            return ret;
        }

        static PortfolioCRUDRequest createPortfolioCRUDRequest(MyPortDTO portFolioDto,string action) {
            PortfolioCRUDRequest request;

            if (string.IsNullOrEmpty(action))
                throw new ArgumentNullException("action","portfolio-action is null!");
            request = new PortfolioCRUDRequest(action,portFolioDto);
            //            request.refKey = Guid.NewGuid().ToString();
            request.refKey = portFolioDto.portRefKey;
            return request;
        }
        #endregion object-creation methods

        MyPortDTO populateDTO(Portfolio aport,out IBasicProperties props,int parentPortNum,bool isLink,List<PortfolioTag> tags) {
            MyPortDTO ret;

            ret = createPort(false);
            if (parentPortNum >= 0) {
                ret.parentPortId = parentPortNum;
                ret.linkInd = isLink ? "Y" : "N";
            } else
                ret.linkInd = "N";
            props = createProps(cc,CRUDE_TYPE_ID);
            ret.desiredPlCurrCode = aport.getDesiredPlCurrCode();
            ret.getOwnerInit = aport.getOwnerInit();
            ret.numHistoryDays = aport.getNumHistoryDays().intValue();
            ret.portClass = aport.getPortClass();
            ret.portFullName = aport.getPortFullName();
            ret.portLocked = aport.getPortLocked().shortValue();
            if (aport.getPortNum() != null)
                ret.portNum = aport.getPortNum().intValue();
            ret.portRefKey = aport.getPortRefKey();
            ret.portShortName = aport.getPortShortName();
            ret.portType = aport.getPortType();
            if (aport.getCmntNum() != null)
                ret.cmntNum = aport.getCmntNum().intValue();
            if (aport.getTradingEntityNum() != null)
                ret.tradingEntityNum = aport.getTradingEntityNum().intValue();
            ret.copyTags(tags);
            return ret;
        }

        static void sendPortfolioCRUDRequest(ChannelContainer cc,PortfolioCRUDRequest request,IBasicProperties props) {
            string jsonST,tmp;
            cc.jss.NullValueHandling = NullValueHandling.Ignore;
            jsonST = JsonConvert.SerializeObject(request,cc.jss);

            tmp = JsonConvert.DeserializeObject(jsonST,cc.jss).ToString();
            Debug.Print("sending request:" + Environment.NewLine + tmp + Environment.NewLine);
            try {
                lock (cc.channelLock)
                    cc.channel.BasicPublish(string.Empty,ROUTING_KEY_CRUD,props,cc.encoding.GetBytes(jsonST));
            } catch (Exception ex) {
                Util.show(MethodBase.GetCurrentMethod(),ex);
            }
        }
    }
}