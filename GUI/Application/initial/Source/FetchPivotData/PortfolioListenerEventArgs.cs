using System;
namespace NSRiskManager {
    public class PortfolioListenerEventArgs : EventArgs {

        #region ctor
        public PortfolioListenerEventArgs(PortfolioTopicResponse ptr) {
            blah = ptr;
        }
        #endregion

        #region properties
        public PortfolioTopicResponse blah { get; private set; }
        #endregion
    }

    public delegate void PortfolioListenerHandler(object sender,PortfolioListenerEventArgs ea);
}