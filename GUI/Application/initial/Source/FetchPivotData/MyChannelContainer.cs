using RabbitMQ.Client;

namespace NSRiskManager {
    class MyChannelContainer : ChannelContainer {
        #region ctor
        public MyChannelContainer(ChannelContainer cc) : base(cc) { }
        #endregion

        #region properties
        public IBasicConsumer consumer { get; set; }
        public IBasicProperties properties { get; set; }
        #endregion
    }
}