using System;
using System.Diagnostics;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using NSRMCommon;

namespace NSRiskManager {
    class RiskMgrWindowData : IXmlSerializable {
        #region constants
        const string HEADER = "RiskMgrWindowData";
        const string MAIN_SPLIT_POS_ATTR = "splitterPosition";
        const string WIN_FRAME_ATTR = "windowFrame";
        const string PORT_PATH_ATTR = "portfolioPath";
        const string LOWER_SPLIT_POS_ATTR = "lowerSplitterPosition";
        const string PNL_SPLIT_POS_ATTR = "pnlSplitterPosition";

        #endregion constants

        #region fields
        IWindowLayoutProvider winLayoutProvider;

        #endregion

        #region ctor

        public RiskMgrWindowData(IWindowLayoutProvider iwlp) {
            if (iwlp == null)
                throw new ArgumentNullException("iwlp",typeof(IWindowLayoutProvider).Name + " is null!");
            winLayoutProvider = iwlp;
        }

        public RiskMgrWindowData(IWindowLayoutProvider iwlp,XmlReader xr)
            : this(iwlp) {
            this.ReadXml(xr);
        }
        #endregion

        #region IXmlSerializable
        public XmlSchema GetSchema() {
            return null;
        }

        public void ReadXml(XmlReader reader) {
            int n,tmp;
            string attrName;

            if (this.winLayoutProvider == null)
                throw new InvalidOperationException("winLayoutProvider is null!");
            reader.MoveToContent();

            if (reader.NodeType == XmlNodeType.Element && string.Compare(reader.Name,HEADER,true) == 0) {
                if (reader.HasAttributes && (n = reader.AttributeCount) > 0) {
                    for (int i = 0;i < n;i++) {
                        reader.MoveToAttribute(i);
                        if (string.Compare(attrName = reader.Name,MAIN_SPLIT_POS_ATTR,true) == 0) {
                            if (Int32.TryParse(reader.Value,out tmp) && tmp > 0)
                                winLayoutProvider.portSplitPosition = tmp;
                        } else if (string.Compare(attrName = reader.Name,LOWER_SPLIT_POS_ATTR,true) == 0) {
                            if (Int32.TryParse(reader.Value,out tmp) && tmp > 0)
                                winLayoutProvider.lowerSplitPosition = tmp;
                        } else if (string.Compare(attrName = reader.Name,PNL_SPLIT_POS_ATTR,true) == 0) {
                            if (Int32.TryParse(reader.Value,out tmp) && tmp > 0)
                                winLayoutProvider.pnlSplitPosition = tmp;
                        } else if (string.Compare(attrName = reader.Name,WIN_FRAME_ATTR,true) == 0) {
                            winLayoutProvider.windowFrame = WindowPositionHelper.makeRectangle(reader.Value);
                        } else if (string.Compare(attrName = reader.Name,PORT_PATH_ATTR,true) == 0) {
                            winLayoutProvider.selectedPortfolioPath = reader.Value;
                        } else {
                            Trace.WriteLine("unhandled attribute: '" + attrName + "'!");
                        }
                    }
                }
            }
        }

        public void WriteXml(XmlWriter writer) {
            if (this.winLayoutProvider == null)
                throw new InvalidOperationException("winLayoutProvider is null!");
            writer.WriteStartDocument(true);
            writer.WriteStartElement(HEADER);
            writer.WriteAttributeString(MAIN_SPLIT_POS_ATTR,this.winLayoutProvider.portSplitPosition.ToString());
            writer.WriteAttributeString(LOWER_SPLIT_POS_ATTR,this.winLayoutProvider.lowerSplitPosition.ToString());
            writer.WriteAttributeString(PNL_SPLIT_POS_ATTR,this.winLayoutProvider.pnlSplitPosition.ToString());
            writer.WriteAttributeString(WIN_FRAME_ATTR,this.winLayoutProvider.windowFrame.ToString());
            writer.WriteAttributeString(PORT_PATH_ATTR,this.winLayoutProvider.selectedPortfolioPath);
            writer.WriteEndDocument();
        }
        #endregion IXmlSerializable
    }
}