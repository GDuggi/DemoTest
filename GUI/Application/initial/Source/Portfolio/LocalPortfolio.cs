#undef NEW_WAY

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using com.amphora.cayenne.entity;
using com.amphora.cayenne.entity.service;
using NSRMCommon;
using NSRMLogging;
using JMAP = java.util.Map;


namespace NSRiskManager {
    class LocalPortfolio {
        #region MyRegion
        public static bool verbose = false;
        List<LocalPortfolio> childPorts;
        bool childrenFetched;
        #endregion

        #region ctor
        public LocalPortfolio(Portfolio portfolio) 
        {
            realPortfolio = portfolio;

            childPorts = new List<LocalPortfolio>();
            try 
            {
                childrenFetched = string.Compare(this.portType,"R") == 0;
            } catch (Exception ex) {
                Util.show(MethodBase.GetCurrentMethod(),ex);
                childrenFetched = false;
            }
        }

        #endregion

        #region properties
        public string IsLinkedIndicator { get; set; }
        public int parentId { get; set; }
        public java.util.Set PortfolioTags { get; set; }
        public Portfolio realPortfolio { get; private set; }
        public int portNum { get { return realPortfolio.getPortNum().intValue(); } }
        public string portType { get { return realPortfolio.getPortType(); } }
        public string portShortName { get { return realPortfolio.getPortShortName(); } }
        #endregion

        #region methods
        public LocalPortfolio[] children() { return childPorts.ToArray(); }
        #endregion

    }
}
