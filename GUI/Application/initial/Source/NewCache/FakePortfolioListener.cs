using System.Reflection;
using com.amphora.entities.container;
using NSRMLogging;
using JLIST = java.util.List;
using JMAP = java.util.Map;

namespace NSRiskManager {
    class FakePortfolioListener : IPortfolioContainer {

        void IPortfolioContainer.deletedPortfolio(JLIST l,string str1,string str2) {
            Util.show(MethodBase.GetCurrentMethod());
        }

        void IPortfolioContainer.insertedPortfolio(JLIST l,JMAP m1,JMAP m2,string str1,string str2) {
            Util.show(MethodBase.GetCurrentMethod());
        }

        void IPortfolioContainer.updatedPortfolio(JLIST l,JMAP m1,JMAP m2,string str1,string str2) {
            Util.show(MethodBase.GetCurrentMethod());
        }

        void IPortfolioContainer.onPortfolioLoaded(java.util.Collection collection)
        {
            Util.show(MethodBase.GetCurrentMethod());
        }

        void IPortfolioContainer.onPortfolioRemoval(com.amphora.entities.dto.IPortfolioEntityDTO dto)
        {
            Util.show(MethodBase.GetCurrentMethod());
        }

       
    }
}