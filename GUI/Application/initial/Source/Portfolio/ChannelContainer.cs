using System;
using System.Reflection;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using NSRMLogging;
using RabbitMQ.Client;

namespace NSRiskManager {
    public class ChannelContainer {
        #region fields
        public readonly Encoding encoding = Encoding.UTF8;
        #endregion

        #region properties
        public IModel channel { get; private set; }
        public JsonSerializerSettings jss { get; private set; }
        public ManualResetEvent dataLock { get; private set; }
        public ManualResetEvent threadLock { get; private set; }
        public bool connected { get; private set; }
        public object channelLock { get; private set; }
        public string correlationId { get; private set; }
        #endregion

        #region ctors
        internal ChannelContainer() {
            channelLock = new object();

            jss = new JsonSerializerSettings();
            jss.Formatting = Formatting.Indented;
            jss.TraceWriter = new MyTraceWriter();
        }

        public ChannelContainer(string acorrid)
            : this() {
            this.correlationId = acorrid;
        }

        protected ChannelContainer(ChannelContainer cc)
            : this() {
            this.channel = cc.channel;
            this.connected = cc.connected;
            this.correlationId = cc.correlationId;
            this.dataLock = cc.dataLock;
            this.threadLock = cc.threadLock;
        }
        #endregion

        internal void setup(IConnection connection) {
            threadLock = new ManualResetEvent(false);
            dataLock = new ManualResetEvent(false);
            try {
                channel = connection.CreateModel();
                connected = true;
            } catch (Exception ex) {
                Util.show(MethodBase.GetCurrentMethod(),ex);
            }
        }

        internal void shutdown() {
            channel.Close();
        }
    }
}