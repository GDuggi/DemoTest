using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBAccess
{
    public class VPcTradeSummaryDto
    {
        public string AdditionalConfirmSent { get; set; }
        public string AnalystName { get; set; }
        public string ArchiveFlag { get; set; }
        public Int64 BkrDbUpd { get; set; }
        public string BkrMeth { get; set; }
        public string BkrRqmt { get; set; }
        public string BkrStatus { get; set; }
        public string Book { get; set; }
        public string BrokerPrice { get; set; }
        public string BrokerSn { get; set; }
        public string BuySellInd { get; set; }
        public string CdtyCode { get; set; }
        public string CdtyGrpCode { get; set; }
        public string Cmt { get; set; }
        public string Comm { get; set; }
        public Int64 CptyDbUpd { get; set; }
        public string CptyLn { get; set; }
        public string CptyMeth { get; set; }
        public string CptyRqmt { get; set; }
        public string CptySn { get; set; }
        public string CptyStatus { get; set; }
        public DateTime CurrentBusnDt { get; set; }
        public string EfsCptySn { get; set; }
        public string EfsFlag { get; set; }
        public DateTime EndDt { get; set; }
        public string FinalApprovalFlag { get; set; }
        public DateTime FinalApprovalTimestampGmt { get; set; }
        public string GroupXref { get; set; }
        public string HasProblemFlag { get; set; }
        public Int64 Id { get; set; }
        public DateTime InceptionDt { get; set; }
        public string IsTestBook { get; set; }
        public DateTime LastTrdEditTimestampGmt { get; set; }
        public DateTime LastUpdateTimestampGmt { get; set; }
        public string LocationSn { get; set; }
        public string MigrateInd { get; set; }
        public Int64 NoconfDbUpd { get; set; }
        public string NoconfMeth { get; set; }
        public string NoconfRqmt { get; set; }
        public string NoconfStatus { get; set; }
        public string OpsDetActFlag { get; set; }
        public string OptnPremPrice { get; set; }
        public string OptnPutCallInd { get; set; }
        public string OptnStrikePrice { get; set; }
        public string PayPrice { get; set; }
        public string PlAmt { get; set; }
        public string PriceDesc { get; set; }
        public string Priority { get; set; }
        public double Qty { get; set; }
        public double QtyTot { get; set; }
        public string ReadyForFinalApprovalFlag { get; set; }
        public string RecPrice { get; set; }
        public Int32 RecentInd { get; set; }
        public string RefSn { get; set; }
        public string RplyRdyToSndFlag { get; set; }
        public string SeCptySn { get; set; }
        public Int64 SetcDbUpd { get; set; }
        public string SetcMeth { get; set; }
        public string SetcRqmt { get; set; }
        public string SetcStatus { get; set; }
        public DateTime StartDt { get; set; }
        public string SttlType { get; set; }
        public DateTime TradeDt { get; set; }
        public Int64 TradeId { get; set; }
        public string TradeStatCode { get; set; }
        public string TradeTypeCode { get; set; }
        public Int64 TransactionSeq { get; set; }
        public string TrdSysCode { get; set; }
        public string UomDurCode { get; set; }
        public Int64 VerblDbUpd { get; set; }
        public string VerblMeth { get; set; }
        public string VerblRqmt { get; set; }
        public string VerblStatus { get; set; }
        public Int32 Version { get; set; }
        public string Xref { get; set; }   
    }
}
