using System.Collections.Generic;
namespace NSRiskManager {
    public class PortfolioRequest : JSONGenerator {

        internal static PortfolioRequest createRequest(int portNum,int[] childPorts) {
            return new PortfolioRequest(portNum,childPorts);
        }

        #region ctor
        public PortfolioRequest() {
            portfolioToExchange = new Dictionary<string,string>();
        }

        PortfolioRequest(int[] childPorts)
            : this() {
            int n;

            if (childPorts != null) {
                this.portfolioIds = new string[n = childPorts.Length];
                if (n > 0)
                    for (int i = 0;i < n;i++)
                        this.portfolioIds[i] = childPorts[i].ToString();

            }
        }

        public PortfolioRequest(int portNum,int[] childPorts)
            : this(childPorts) {
            parentPortfolioId = portNum.ToString();
        }

        public PortfolioRequest(string p1,int[] p2)
            : this(p2) {
            this.userid = p1;
        }

        #endregion

        #region properties
        public string userid { get; set; }
        public string parentPortfolioId { get; set; }
        public string[] portfolioIds { get; set; }
        public IDictionary<string,string> portfolioToExchange { get; set; }
        #endregion
    }
}