namespace NSRiskManager {
    public class PortfolioTopicRequest {
        #region ctor
        public PortfolioTopicRequest(string userid,string[] portNumbers) {
            this.userId = userid;
            this.portfolioIds = portNumbers;
        }
        #endregion
        #region properties
        public string userId { get; set; }
        public string[] portfolioIds { get; set; }
        #endregion
    }
}