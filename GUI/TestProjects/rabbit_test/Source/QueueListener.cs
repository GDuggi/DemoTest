//#undef ALL_EVENTS

using System;
using System.Reflection;
using System.Text;
using System.Threading;
using NSRMCommon;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Framing;

namespace NSRabbit_test {
    class QueueListener : IDisposable {
        #region fields
        IConnectionFactory listenerFactory;
        IConnection listenerConnection;
        IModel listenerModel;
        Thread t;
        string listenQName;
        #endregion

        #region ctor
        public QueueListener(string exchName,string exchType,IStatusHandler ish) {
            if (ish == null)
                throw new ArgumentNullException("ish",typeof(IStatusHandler).Name + " is null!");
            statusHandler = ish;
            if (string.IsNullOrEmpty(exchName))
                throw new ArgumentNullException("exchName","exchange-name is null!");
            exchangeName = exchName;
            if (string.IsNullOrEmpty(exchType))
                throw new ArgumentNullException("exchType","exchange-type is null!");
            exchangeType = exchType;
        }
        #endregion

        #region properties
        public string exchangeName { get;  set; }
        public string exchangeType { get;  set; }
        public bool running { get;  set; }
        IStatusHandler statusHandler { get; set; }
        #endregion

        public void Dispose() {
            Util.show(MethodBase.GetCurrentMethod());
        }


        ManualResetEvent mre;

        internal void start() {
            // begin a thread.

            mre = new ManualResetEvent(true);
            try {
                listenerFactory = QueueUtil.createFactory();
                listenerConnection = listenerFactory.CreateConnection();

                listenerModel = listenerConnection.CreateModel();
                listenerModel.ExchangeDeclare(exchangeName,exchangeType);

                listenQName = listenerModel.QueueDeclare().QueueName;
                listenerModel.QueueBind(listenQName,exchangeName,string.Empty);

                var consumer = new QueueingBasicConsumer(listenerModel);

#if ALL_EVENTS
                listenerConnection.ConnectionShutdown += listenerConnection_ConnectionShutdown;
                listenerModel.ModelShutdown += listenerModel_ModelShutdown;
                consumer.ConsumerCancelled += consumer_ConsumerCancelled;
#endif
                listenerModel.BasicConsume(listenQName,true,consumer);

                Console.WriteLine(" [*] Waiting for logs." +
                                     "To exit press CTRL+C");
                t = new Thread(new ParameterizedThreadStart(readQueue));
                t.Start(new ListenerData(consumer,this.statusHandler));
            } catch (Exception ex) {
                Util.show(MethodBase.GetCurrentMethod(),ex);
            }
        }

        internal void stop() {
            // shut down the thread.
            if (mre != null)  
                mre.Reset();
            listenerModel.ExchangeUnbind(listenQName,exchangeName,string.Empty);
            listenerModel.Close(Constants.ReplySuccess,"OK");
            listenerModel.Dispose();
            listenerModel = null;

            listenerConnection.Close(Constants.ReplySuccess,"OK");
            listenerConnection.Dispose();
            listenerConnection = null;

            listenerFactory = null;

        }

        void readQueue(object data) {
            ListenerData ld;

            if (data == null)
                throw new ArgumentNullException("data","listener-thread-data is null!");
            if (!(data is ListenerData))
                throw new InvalidOperationException("expected parameter of type " + typeof(ListenerData).Name + ", received " + data.GetType().Name);

            ld = data as ListenerData;
            while (true) {

                try {
                    var ea = (BasicDeliverEventArgs)ld.consumer.Queue.Dequeue();

                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    ld.statusHandler.showStatus("received: " + message);
                } catch(System.IO.EndOfStreamException eose){
                    ld.statusHandler.showError("error: " + Util.makeErrorMessage(eose));
                } catch (Exception ex) {
                    ld.statusHandler.showError("error: " + Util.makeErrorMessage(ex));
                }
                if (!mre.WaitOne(100)) {
                    ld.statusHandler.showStatus("listen-loop terminated.");
                    break;
                }
            }
            ld.statusHandler.showStatus("leaving " + Util.makeSig(MethodBase.GetCurrentMethod()));
        }

#if ALL_EVENTS
        void consumer_ConsumerCancelled(object sender,ConsumerEventArgs args) {
            Util.show(MethodBase.GetCurrentMethod());
        }

        void listenerModel_ModelShutdown(IModel model,ShutdownEventArgs reason) {
            Util.show(MethodBase.GetCurrentMethod());
        }

        void listenerConnection_ConnectionShutdown(IConnection connection,ShutdownEventArgs reason) {
            Util.show(MethodBase.GetCurrentMethod());
        }
#endif
    }

    class ListenerData {
        #region ctor
        internal ListenerData(QueueingBasicConsumer qbc,IStatusHandler ish) {
            if (qbc == null)
                throw new ArgumentNullException("qbc",typeof(QueueingBasicConsumer).Name + " is null!");
            consumer = qbc;
            if (qbc == null)
                throw new ArgumentNullException("ish",typeof(IStatusHandler).Name + " is null!");
            statusHandler = ish;
        }
        #endregion

        #region MyRegion
        public QueueingBasicConsumer consumer { get;  set; }
        public IStatusHandler statusHandler { get;  set; }
        #endregion
    }
}