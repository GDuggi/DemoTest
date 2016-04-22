using System.Reflection;
using System.Threading;
using NSRMLogging;
using RabbitMQ.Client;

namespace NSRiskManager {
    class TopicPerPortfolioThread {
        #region fields
        static int threadCtr = 0;
        IConnection connection;
        string p;
        #endregion

        #region ctor
        public TopicPerPortfolioThread(IConnection connection,string p) {
            this.connection = connection;
            this.p = p;
        }
        #endregion

        internal void run() {
            if (string.IsNullOrEmpty(Thread.CurrentThread.Name))
                Thread.CurrentThread.Name = "TopicThread_" + (++threadCtr);
            Util.show(MethodBase.GetCurrentMethod());
        }
    }
}