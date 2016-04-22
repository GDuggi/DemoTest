using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.amphora.cayenne.entity;
using System.ComponentModel;
using NSRMCommon;

namespace NSRiskMgrCtrls
{
    public class WrappedInventoryBuildDraw
    {
        #region fields
        InventoryBuildDraw invBD;
        #endregion

        #region ctor
        public WrappedInventoryBuildDraw(InventoryBuildDraw inBD) { invBD = inBD; }
        #endregion

        #region properties
        [DisplayName("Inventory Num")]
        public int invNum { get { return JavaDBUtil.copyInt(invBD.getInvNum()); } }
        [DisplayName("Type")]
        public string invBDType { get { return invBD.getInvBDType(); } }
        [DisplayName("Quantity")]
        public double invBDQty { get { return JavaDBUtil.copyDouble(invBD.getInvBDQty()); } }
        [DisplayName("Uom")]
        public string invBDCostUomCode 
        { 
            get 
            {
                if (invBD.getInvBDCostUomCodeToUom() != null)
                    return invBD.getInvBDCostUomCodeToUom().getUomCode();
                else
                    return null;
            }
        }
        [DisplayName("Allocation Num")]
        public int allocNum { get { return JavaDBUtil.copyInt(invBD.getAllocNum()); } }
        [DisplayName("Allocation Item Num")]
        public int allocItemNum { get { return JavaDBUtil.copyShort(invBD.getAllocItemNum()); } }
        [DisplayName("Trade Num")]
        public int tradeNum { get { return JavaDBUtil.copyInt(invBD.getTradeNum()); } }
        [DisplayName("Order Num")]
        public int orderNum { get { return JavaDBUtil.copyShort(invBD.getOrderNum()); } }
        [DisplayName("Item Num")]
        public int itemNum { get { return JavaDBUtil.copyShort(invBD.getItemNum()); } }


        [Browsable(false)]
        public double adjQty { get { return JavaDBUtil.copyDouble(invBD.getAdjQty()); } }
        [Browsable(false)]
        public string adjTypeInd { get { return invBD.getAdjTypeInd(); } }
        [Browsable(false)]
        public string associatedTrade { get { return invBD.getAssociatedTrade(); } }
        [Browsable(false)]
        public int cmntNum { get { return JavaDBUtil.copyInt(invBD.getCmntNum()); } }
        [Browsable(false)]
        public double invBDActualQty { get { return JavaDBUtil.copyDouble(invBD.getInvBDActualQty()); } }
        [Browsable(false)]
        public double invBDCost { get { return JavaDBUtil.copyDouble(invBD.getInvBDCost()); } }
        [Browsable(false)]
        public double invBDCostWacog { get { return JavaDBUtil.copyDouble(invBD.getInvBDCostWacog()); } }
        [Browsable(false)]
        public DateTime invBDDate { get { return JavaDBUtil.makeDatetime(invBD.getInvBDDate()); } }
        [Browsable(false)]
        public int invBDNum { get { return JavaDBUtil.copyInt(invBD.getInvBDNum()); } }
        [Browsable(false)]
        public string invBDStatus { get { return invBD.getInvBDStatus(); } }
        [Browsable(false)]
        public string invBDTaxStatusCode { get { return invBD.getInvBDTaxStatusCode(); } }
        [Browsable(false)]
        public double invCurrActualQty { get { return JavaDBUtil.copyDecimal(invBD.getInvCurrActualQty()); } }
        [Browsable(false)]
        public double invCurrProjQty { get { return JavaDBUtil.copyDecimal(invBD.getInvCurrProjQty()); } }
        [Browsable(false)]
        public int invDrawBDNum { get { return JavaDBUtil.copyShort(invBD.getInvDrawBDNum()); } }
        [Browsable(false)]
        public int posGroupNum { get { return JavaDBUtil.copyInt(invBD.getPosGroupNum()); } }
        [Browsable(false)]
        public double rInvBDCost { get { return JavaDBUtil.copyDouble(invBD.getRInvBDCost()); } }
        [Browsable(false)]
        public double rInvBDCostWacog { get { return JavaDBUtil.copyDouble(invBD.getRInvBDCostWacog()); } }
        [Browsable(false)]
        public int transId { get { return JavaDBUtil.copyInt(invBD.getTransId()); } }
        [Browsable(false)]
        public double unrInvBDCost { get { return JavaDBUtil.copyDouble(invBD.getUnrInvBDCost()); } }
        [Browsable(false)]
        public double unrInvBDCostWacog { get { return JavaDBUtil.copyDouble(invBD.getUnrInvBDCostWacog()); } }


        #endregion
    }
}
