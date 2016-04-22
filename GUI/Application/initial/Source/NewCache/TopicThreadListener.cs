using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using com.amphora.events.listener;
using NSRMLogging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace NSRiskManager {
    public class TopicThreadListener {
        #region constants
        public const string ROUTING_KEY_CRUD = "portfolio.crud";
        #endregion

        #region fields
        readonly ManualResetEvent mre = new ManualResetEvent(false);
        readonly object localLock = new object();
        QueueingBasicConsumer consumer;
        Thread thread;
        MyChannelContainer sharedContainer;
        #endregion

        #region properties
        public string topic { get; private set; }
        ForwardRabbitMsg forwardTarget { get; set; }
        public Guid guid { get; private set; }
        public IRabbitListener listener { get; private set; }
        internal int useCount { get; set; }
        #endregion

        #region ctor
        internal TopicThreadListener(string aTopic,ForwardRabbitMsg forwardRabbitMsg,IRabbitListener irl) {
            this.topic = aTopic;
            this.forwardTarget = forwardRabbitMsg;
            listener = irl;
            guid = Guid.NewGuid();
        }
        #endregion

        #region methods

        internal static IBasicProperties createProperties(MyChannelContainer mcc) {
            IBasicProperties props;

            if (mcc == null)
                throw new ArgumentNullException("cc",typeof(ChannelContainer).FullName + " is null!");
            if (mcc.channel == null)
                throw new ArgumentNullException("cc.channel",typeof(IModel).FullName + " is null!");
            if (mcc.channelLock == null)
                throw new ArgumentNullException("cc.channelLock","channel-lock is null!");
            lock (mcc.channelLock)
                props = mcc.channel.CreateBasicProperties();
            props.ContentType = Util.CONTENT_TYPE;
            props.DeliveryMode = 2;
            return props;
        }

       

        internal void start(MyChannelContainer mcc) {
            sharedContainer = mcc;
            this.thread = new Thread(new ParameterizedThreadStart(listenOnChannel));
            thread.Start(mcc);
        }

        internal void shutdown(MyChannelContainer mcc) {
            this.mre.Set();
            this.mre.WaitOne();
            this.thread.Join();
            sharedContainer = null;
        }

        void listenOnChannel(object anObj) {
            BasicDeliverEventArgs DefaultDlvArgs = new BasicDeliverEventArgs(),delivery;
            ChannelContainer cc;
            string name,response;

            if (anObj == null)
                throw new ArgumentNullException("anObj","channel is null!");
            if (!(anObj is ChannelContainer))
                throw new ArgumentNullException();
            try {
                if (string.IsNullOrEmpty(Thread.CurrentThread.Name))
                    Thread.CurrentThread.Name = this.topic + "-listener-thread";
            } catch (Exception ex) {
                Util.show(MethodBase.GetCurrentMethod(),ex);
            }
            cc = anObj as ChannelContainer;

            lock (cc.channelLock) {
                lock (this.localLock)
                    consumer = new QueueingBasicConsumer(cc.channel);
                cc.channel.ExchangeDeclare(this.topic,"topic",false,true,null);
                name = cc.channel.QueueDeclare().QueueName;
                cc.channel.QueueBind(name,this.topic,string.Empty);
                cc.channel.BasicConsume(name,true,consumer);
            }
            Util.show(MethodBase.GetCurrentMethod(),"start listening for '" + this.topic + "' [" + name + "].");
            
            while (true) 
            {
                
                if (mre.WaitOne(100)) 
                {
                    break;
                } 
                else 
                {
                    lock (this.localLock)
                        delivery = consumer.Queue.DequeueNoWait(DefaultDlvArgs);
                    if (delivery.Equals(DefaultDlvArgs))
                        Thread.Sleep(100);
                    else {
                        response = cc.encoding.GetString(delivery.Body);

                       
                       
                        try {
                            lock (this.localLock)
                                if (this.forwardTarget != null)
                                    this.forwardTarget(this,listener,topic,response);
                        } catch (Exception ex) {
                            Util.show(MethodBase.GetCurrentMethod(),ex);
                        }
                    }
                }
            }
            if (consumer.Queue != null)
                consumer.Queue.Close();

        }
        #endregion methods

    }
}