#define NEW_TAG_STRUCTURE

using System;
using System.Collections.Generic;
using System.Diagnostics;
using com.amphora.cayenne.entity;

namespace NSRiskManager {
    public class MyPortDTO {
        public string linkInd;
        public int cmntNum;
        public int numHistoryDays;
        public int parentPortId;
        public int portNum;
        public int tradingEntityNum;
        public int transId;
        public short portLocked;
        public string desiredPlCurrCode;
        public string getOwnerInit;
        public string portClass;
        public string portFullName;
        public string portRefKey;
        public string portShortName;
        public string portType;

        public MyTagContainer portfolioTags;

        public MyPortDTO() {
            cmntNum = numHistoryDays = parentPortId = portNum = tradingEntityNum = transId = int.MinValue;
            portLocked = -1;
            cmntNum = 0;

            portfolioTags = new MyTagContainer();

        }

        public MyPortDTO(string p)
            : this() {
            if (string.IsNullOrEmpty(p))
                throw new ArgumentNullException("p","port-name is null!");
            this.portShortName = this.portFullName = p;
        }

        public void copyTags(List<PortfolioTag> tags) {
            foreach (var aTag in tags)

                this.portfolioTags.tags.Add(aTag.getTagName(),aTag.getTagValue());

        }
    }

#if NEW_TAG_STRUCTURE
    public class MyTagContainer {

        public MyTagContainer() { tags = new Dictionary<string,string>(); }
        public IDictionary<string,string> tags { get; set; }
    }
#endif
}