using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using NSRMLogging;
using RabbitMQ.Client;

namespace NSRiskManager {
    class TopicChannelData : IDisposable {
        #region fields
        public readonly object modelLock = new object();
        public readonly Encoding encoding = Encoding.UTF8;
        #endregion

        #region properties
        public IModel model { get; private set; }
        public ManualResetEvent signal { get; private set; }
        public bool wasSignaled { get; private set; }
        public bool verbose { get; set; }
        public bool disposed { get; private set; }
        public JsonSerializerSettings jss { get; private set; }
        #endregion

        #region ctors
        TopicChannelData() {
            signal = new ManualResetEvent(false);
            jss = new JsonSerializerSettings();
            jss.TraceWriter = new MyTraceWriter(TraceLevel.Warning);
            jss.Formatting = Formatting.Indented;
            jss.NullValueHandling = NullValueHandling.Ignore;
        }

        public TopicChannelData(IModel amodel)
            : this() {
            if (amodel == null)
                throw new ArgumentNullException("amodel",typeof(IModel).FullName + " is null!");
            model = amodel;
        }
        #endregion

        #region methods

        internal bool threadDone() {
            if (signal.WaitOne(10)) {
#if VERBOSE
Util.show(MethodBase.GetCurrentMethod(),"position-item signalled");
#endif
                signal.Reset();
                wasSignaled = true;
            }
            return wasSignaled;
        }
        public void Dispose() {
            Util.show(MethodBase.GetCurrentMethod());
            if (!disposed) {
                disposed = true;
                Util.show(MethodBase.GetCurrentMethod());
            }
        }
        #endregion
    }
}