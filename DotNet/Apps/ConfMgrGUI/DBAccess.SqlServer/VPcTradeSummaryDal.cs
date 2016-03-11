using OpsTrackingModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;

namespace DBAccess.SqlServer
{
    public class VPcTradeSummaryDal : IVPcTradeSummaryDal
    {
        private string sqlConnStr = "";

        public VPcTradeSummaryDal(string pSqlConn)
        {
            sqlConnStr = pSqlConn;
        }

        #region Stubbed Data

        public List<SummaryData> GetAllStub()
        {
            //string dateStr = "";
            var result = new List<SummaryData>();
            result.Add(new SummaryData
            {
                TrdSysCode = "AFF",
                TradeSysTicket = "TRD17-1585-01",
                Version = 1,
                CurrentBusnDt = DateTime.ParseExact("04-14-2015", "MM-dd-yyyy", CultureInfo.InvariantCulture),
                RecentInd = 0,
                LastUpdateTimestampGmt = DateTime.ParseExact("03-25-2015", "MM-dd-yyyy", CultureInfo.InvariantCulture),
                LastTrdEditTimestampGmt = DateTime.ParseExact("03-25-2015", "MM-dd-yyyy", CultureInfo.InvariantCulture),
                ReadyForFinalApprovalFlag = "Y",
                HasProblemFlag = "N",
                FinalApprovalFlag = "N",
                OpsDetActFlag = "N",
                TransactionSeq = 1003766,
                NoconfRqmt = "NOCNF",
                NoconfMeth = "OTHER",
                NoconfStatus = "APPR",
                NoconfDbUpd = 270328,
                Id = 149483,
                TradeId = 1483078,
                InceptionDt = DateTime.ParseExact("03-25-2015", "MM-dd-yyyy", CultureInfo.InvariantCulture),
                CdtyCode = "ELEC",
                TradeDt = DateTime.ParseExact("03-25-2015", "MM-dd-yyyy", CultureInfo.InvariantCulture),
                Xref = "TP4019228",
                CptySn = "JPM SECURI",
                CptyLegalName = "JP MORGAN SECURITIES PLC - UK",
                CptyTradeId = "RWS-8820",
                QtyTot = 300,
                PermissionKey = "AMPH US",
                //Qty = 25,
                //UomDurCode = "MW/PKHR",
                LocationSn = "ELEC GERMAN SWAP",
                StartDt = DateTime.ParseExact("03-26-2015", "MM-dd-yyyy", CultureInfo.InvariantCulture),
                EndDt = DateTime.ParseExact("03-26-2015", "MM-dd-yyyy", CultureInfo.InvariantCulture),
                Book = "CONTI POWER",
                TradeTypeCode = "ENRGY",
                SttlType = "FNCL",
                BuySellInd = "S",
                //PayPrice = "EPEX GERMAN SPOT",
                //RecPrice = "48.0000 EUR/MW",
                BookingCoSn = "AMPH US",
                //SeCptySn = "AMPH US",
                TradeStatCode = "OPEN",
                TransportDesc = "",
                CdtyGrpCode = "ELEC",
                Priority = "4",
                PlAmt = "0.00",
                //EfsFlag = "N",
                ArchiveFlag = "N",
                RplyRdyToSndFlag = "N",
                MigrateInd = "G",
                AnalystName = "TBD",
                IsTestBook = "N"
            });

            result.Add(new SummaryData
            {
                TrdSysCode = "AFF",
                Version = 1,
                CurrentBusnDt = DateTime.ParseExact("04-14-2015", "MM-dd-yyyy", CultureInfo.InvariantCulture),
                RecentInd = 0,
                LastUpdateTimestampGmt = DateTime.ParseExact("04-13-2015 08:15:24", "MM-dd-yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                LastTrdEditTimestampGmt = DateTime.ParseExact("04-09-2015 08:15:24", "MM-dd-yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                ReadyForFinalApprovalFlag = "N",
                HasProblemFlag = "N",
                FinalApprovalFlag = "N",
                OpsDetActFlag = "N",
                TransactionSeq = 1025915,
                SetcRqmt = "XQCSP",
                SetcMeth = "Our Paper",
                SetcStatus = "SENT",
                SetcDbUpd = 279970,
                CptyRqmt = "XQCCP",
                CptyMeth = "Cpty Paper",
                CptyStatus = "EXPCT",
                CptyDbUpd = 279080,
                CptyTradeId = "CIT-150",
                Id = 155037,
                TradeId = 1493829,
                InceptionDt = DateTime.ParseExact("04-09-2015", "MM-dd-yyyy", CultureInfo.InvariantCulture),
                CdtyCode = "NGAS",
                TradeDt = DateTime.ParseExact("04-09-2015", "MM-dd-yyyy", CultureInfo.InvariantCulture),
                CptySn = "MMGS INC",
                CptyLegalName = "MMGS INC",
                QtyTot = 610000,
                PermissionKey = "AMPH US",
                //Qty = 5000,
                //UomDurCode = "MMBTU/DAY",
                LocationSn = "COLUMBIA-TCO",
                StartDt = DateTime.ParseExact("12-01-2015", "MM-dd-yyyy", CultureInfo.InvariantCulture),
                EndDt = DateTime.ParseExact("03-31-2016", "MM-dd-yyyy", CultureInfo.InvariantCulture),
                Book = "USPHYS",
                TradeTypeCode = "ENRGY",
                SttlType = "PHYS",
                BuySellInd = "B",
                //PayPrice = "NYMEX:NGAS L1D -0.22 USD/MMBTU",
                BookingCoSn = "BTGPC US",
                //SeCptySn = "BTGPC US",
                TradeStatCode = "OPEN",
                TransportDesc = "Pipeline",
                CdtyGrpCode = "NGAS",
                Priority = "4",
                PlAmt = "0.00",
                //EfsFlag = "N",
                ArchiveFlag = "N",
                RplyRdyToSndFlag = "N",
                MigrateInd = "G",
                AnalystName = "TBD",
                IsTestBook = "N"
            });

            result.Add(new SummaryData
            {
                TrdSysCode = "AFF",
                Version = 1,
                CurrentBusnDt = DateTime.ParseExact("04-14-2015", "MM-dd-yyyy", CultureInfo.InvariantCulture),
                RecentInd = 0,
                LastUpdateTimestampGmt = DateTime.ParseExact("04-28-2015", "MM-dd-yyyy", CultureInfo.InvariantCulture),
                LastTrdEditTimestampGmt = DateTime.ParseExact("04-22-2015", "MM-dd-yyyy", CultureInfo.InvariantCulture),
                ReadyForFinalApprovalFlag = "Y",
                HasProblemFlag = "N",
                FinalApprovalFlag = "N",
                OpsDetActFlag = "N",
                TransactionSeq = 1026354,
                NoconfRqmt = "NOCNF",
                NoconfMeth = "CPEXC",
                NoconfStatus = "APPR",
                NoconfDbUpd = 280559,
                Id = 156136,
                TradeId = 1495866,
                InceptionDt = DateTime.ParseExact("04-14-2015", "MM-dd-yyyy", CultureInfo.InvariantCulture),
                CdtyCode = "NGAS",
                TradeDt = DateTime.ParseExact("04-14-2015", "MM-dd-yyyy", CultureInfo.InvariantCulture),
                CptySn = "MIZUHO",
                CptyLegalName = "MIZUHO SECURITIES USA INC",
                CptyTradeId = "CIT-9925-15",
                QtyTot = 0,
                PermissionKey = "MERC US",
                //Qty = 1000000,
                //UomDurCode = "MMBTU/DAY",
                LocationSn = "SOCAL-STORAGE",
                StartDt = DateTime.ParseExact("04-14-2015", "MM-dd-yyyy", CultureInfo.InvariantCulture),
                EndDt = DateTime.ParseExact("04-30-2015", "MM-dd-yyyy", CultureInfo.InvariantCulture),
                Book = "BTGTEST",
                TradeTypeCode = "STRG",
                SttlType = "PHYS",
                BuySellInd = "B",
                BookingCoSn = "BTGPC US",
                //SeCptySn = "BTGPC US",
                TradeStatCode = "pend",
                TransportDesc = "Ship",
                CdtyGrpCode = "NGAS",
                Priority = "4",
                PlAmt = "0.00",
                //EfsFlag = "N",
                ArchiveFlag = "N",
                RplyRdyToSndFlag = "N",
                MigrateInd = "G",
                AnalystName = "TBD",
                IsTestBook = "N"
            });

            return result;
        }

        #endregion

        public List<string> GetAllTradingSysCodes(string pPermissionKeyInClause)
        {
            var result = new List<string>();
            string sql = "select distinct TRD_SYS_CODE from " + DBUtils.SCHEMA_NAME + "v_pc_trade_summary ";
                        //" where final_approval_flag = 'N' ";

            if (pPermissionKeyInClause != "")
                sql += " where " + pPermissionKeyInClause;

            sql += " order by TRD_SYS_CODE ";

            using (SqlConnection conn = new SqlConnection(sqlConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    conn.Open();
                    using (SqlDataReader dataReader = cmd.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                result.Add(dataReader["TRD_SYS_CODE"].ToString());
                            }
                        }
                    }
                }
            }
            return result;
        }


        public List<SummaryData> GetAll(string pPermissionKeyInClause)
        {
            var result = new List<SummaryData>();
            string sql = "select * from " + DBUtils.SCHEMA_NAME + "v_pc_trade_summary " +
                        " where final_approval_flag = 'N' ";

            //Naveen 10/25/2015 -- Added support for Null permission key
            if (pPermissionKeyInClause != "")
                sql += " and " +"( " + pPermissionKeyInClause +" or PERMISSION_KEY is null )";

            using (SqlConnection conn = new SqlConnection(sqlConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    conn.Open();
                    using (SqlDataReader dataReader = cmd.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                result.Add(new SummaryData
                                {
                                    AdditionalConfirmSent = dataReader["ADDITIONAL_CONFIRM_SENT"].ToString(),
                                    AnalystName = dataReader["ANALYST_NAME"].ToString(),
                                    ArchiveFlag = dataReader["ARCHIVE_FLAG"].ToString(),
                                    BookingCoId = DBUtils.HandleInt32IfNull(dataReader["BOOKING_CO_ID"].ToString()),
                                    BookingCoSn = dataReader["BOOKING_CO_SN"].ToString(),
                                    //BookingCoSn = dataReader["SE_CPTY_SN"].ToString(),
                                    BkrDbUpd = DBUtils.HandleInt32IfNull(dataReader["BKR_DB_UPD"].ToString()),
                                    BkrMeth = dataReader["BKR_METH"].ToString(),
                                    BkrRqmt = dataReader["BKR_RQMT"].ToString(),
                                    BkrStatus = dataReader["BKR_STATUS"].ToString(),
                                    Book = dataReader["BOOK"].ToString(),
                                    BrokerId = DBUtils.HandleInt32IfNull(dataReader["BROKER_ID"].ToString()),
                                    BrokerLegalName = dataReader["BROKER_LEGAL_NAME"].ToString(),
                                    BrokerPrice = dataReader["BROKER_PRICE"].ToString(),
                                    BrokerSn = dataReader["BROKER_SN"].ToString(),
                                    BuySellInd = dataReader["BUY_SELL_IND"].ToString(),
                                    CdtyCode = dataReader["CDTY_CODE"].ToString(),
                                    CdtyGrpCode = dataReader["CDTY_GRP_CODE"].ToString(),
                                    Cmt = dataReader["CMT"].ToString(),
                                    //Comm = dataReader["COMM"].ToString(),
                                    CptyDbUpd = DBUtils.HandleInt32IfNull(dataReader["CPTY_DB_UPD"].ToString()),
                                    CptyId = DBUtils.HandleInt32IfNull(dataReader["CPTY_ID"].ToString()),
                                    CptyLegalName = dataReader["CPTY_LEGAL_NAME"].ToString(),
                                    //CptyLn = dataReader["CPTY_LN"].ToString(),
                                    CptyMeth = dataReader["CPTY_METH"].ToString(),
                                    CptyRqmt = dataReader["CPTY_RQMT"].ToString(),
                                    CptySn = dataReader["CPTY_SN"].ToString(),
                                    CptyStatus = dataReader["CPTY_STATUS"].ToString(),
                                    CptyTradeId = dataReader["CPTY_TRADE_ID"].ToString(),
                                    CurrentBusnDt = DBUtils.HandleDateTimeIfNull(dataReader["CURRENT_BUSN_DT"].ToString()),
                                    //EfsCptySn = dataReader["EFS_CPTY_SN"].ToString(),
                                    //EfsFlag = dataReader["EFS_FLAG"].ToString(),
                                    EndDt = DBUtils.HandleDateTimeIfNull(dataReader["END_DT"].ToString()),
                                    FinalApprovalFlag = dataReader["FINAL_APPROVAL_FLAG"].ToString(),
                                    FinalApprovalTimestampGmt = DBUtils.HandleDateTimeIfNull(dataReader["FINAL_APPROVAL_TIMESTAMP_GMT"].ToString()),
                                    GroupXref = dataReader["GROUP_XREF"].ToString(),
                                    HasProblemFlag = dataReader["HAS_PROBLEM_FLAG"].ToString(),
                                    Id = DBUtils.HandleInt32IfNull(dataReader["ID"].ToString()),
                                    InceptionDt = DBUtils.HandleDateTimeIfNull(dataReader["INCEPTION_DT"].ToString()),
                                    IsTestBook = dataReader["IS_TEST_BOOK"].ToString(),
                                    LastTrdEditTimestampGmt = DBUtils.HandleDateTimeIfNull(dataReader["LAST_TRD_EDIT_TIMESTAMP_GMT"].ToString()),
                                    LastUpdateTimestampGmt = DBUtils.HandleDateTimeIfNull(dataReader["LAST_UPDATE_TIMESTAMP_GMT"].ToString()),
                                    LocationSn = dataReader["LOCATION_SN"].ToString(),
                                    MigrateInd = dataReader["MIGRATE_IND"].ToString(),
                                    NoconfDbUpd = DBUtils.HandleInt32IfNull(dataReader["NOCONF_DB_UPD"].ToString()),
                                    NoconfMeth = dataReader["NOCONF_METH"].ToString(),
                                    NoconfRqmt = dataReader["NOCONF_RQMT"].ToString(),
                                    NoconfStatus = dataReader["NOCONF_STATUS"].ToString(),
                                    OpsDetActFlag = dataReader["OPS_DET_ACT_FLAG"].ToString(),
                                    OptnPremPrice = dataReader["OPTN_PREM_PRICE"].ToString(),
                                    OptnPutCallInd = dataReader["OPTN_PUT_CALL_IND"].ToString(),
                                    OptnStrikePrice = dataReader["OPTN_STRIKE_PRICE"].ToString(),
                                    //PayPrice = dataReader["PAY_PRICE"].ToString(),
                                    PlAmt = dataReader["PL_AMT"].ToString(),
                                    PermissionKey = dataReader["PERMISSION_KEY"].ToString(),
                                    PriceDesc = dataReader["PRICE_DESC"].ToString(),
                                    Priority = dataReader["PRIORITY"].ToString(),
                                    //Qty = DBUtils.HandleFloatIfNull(dataReader["QTY"].ToString()),
                                    QuantityDescription = dataReader["QTY_DESC"].ToString(),
                                    QtyTot = DBUtils.HandleFloatIfNull(dataReader["QTY_TOT"].ToString()),
                                    ReadyForFinalApprovalFlag = dataReader["READY_FOR_FINAL_APPROVAL_FLAG"].ToString(),
                                    //RecPrice = dataReader["REC_PRICE"].ToString(),
                                    RecentInd = DBUtils.HandleInt32IfNull(dataReader["RECENT_IND"].ToString()),
                                    RefSn = dataReader["REF_SN"].ToString(),
                                    RplyRdyToSndFlag = dataReader["RPLY_RDY_TO_SND_FLAG"].ToString(),
                                    //SeCptySn = dataReader["SE_CPTY_SN"].ToString(),
                                    SetcDbUpd = DBUtils.HandleInt32IfNull(dataReader["SETC_DB_UPD"].ToString()),
                                    SetcMeth = dataReader["SETC_METH"].ToString(),
                                    SetcRqmt = dataReader["SETC_RQMT"].ToString(),
                                    SetcStatus = dataReader["SETC_STATUS"].ToString(),
                                    StartDt = DBUtils.HandleDateTimeIfNull(dataReader["START_DT"].ToString()),
                                    SttlType = dataReader["STTL_TYPE"].ToString(),
                                    TradeDt = DBUtils.HandleDateTimeIfNull(dataReader["TRADE_DT"].ToString()),
                                    TradeDesc = dataReader["TRADE_DESC"].ToString(),
                                    TradeId = DBUtils.HandleInt32IfNull(dataReader["TRADE_ID"].ToString()),
                                    Trader = dataReader["TRADER"].ToString(),
                                    TradeStatCode = dataReader["TRADE_STAT_CODE"].ToString(),
                                    TradeTypeCode = dataReader["TRADE_TYPE_CODE"].ToString(),
                                    TransactionSeq = DBUtils.HandleInt32IfNull(dataReader["TRANSACTION_SEQ"].ToString()),
                                    TransportDesc = dataReader["TRANSPORT_DESC"].ToString(),
                                    TrdSysCode = dataReader["TRD_SYS_CODE"].ToString(),
                                    TradeSysTicket = dataReader["TRD_SYS_TICKET"].ToString(),
                                    //UomDurCode = dataReader["UOM_DUR_CODE"].ToString(),
                                    VerblDbUpd = DBUtils.HandleInt32IfNull(dataReader["VERBL_DB_UPD"].ToString()),
                                    VerblMeth = dataReader["VERBL_METH"].ToString(),
                                    VerblRqmt = dataReader["VERBL_RQMT"].ToString(),
                                    VerblStatus = dataReader["VERBL_STATUS"].ToString(),
                                    Version = DBUtils.HandleInt32IfNull(dataReader["VERSION"].ToString()),
                                    Xref = dataReader["XREF"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            return result;
        }

        public Int32 GetAllTradeIdCount(string pTrdSysCode, string pSeCptySn, string pCptySn, string pCdtyCode,
            DateTime pBeginTradeDt, DateTime pEndTradeDt, string pTrdSysTicket, string pCptyTradeId, string pPermissionKeyInClause)
        {
            const string SELECT_OBJECT = "count(*)";
            Int32 result = 0;
            string sql = GetAllTradeSummarySqlCommandStr(pTrdSysCode, pSeCptySn, pCptySn, pCdtyCode,
                        pBeginTradeDt, pEndTradeDt, pTrdSysTicket, pCptyTradeId, SELECT_OBJECT);

            if (pPermissionKeyInClause != "")
                //Israel 11/20/2015 -- Added support for Null permission key
                //sql += " and " + pPermissionKeyInClause;
                sql += " and " + "( " + pPermissionKeyInClause + " or PERMISSION_KEY is null )";

            using (SqlConnection conn = new SqlConnection(sqlConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    conn.Open();
                    result = (Int32)cmd.ExecuteScalar();
                }
            }
            return result;
        }

        public List<SummaryData> GetAllSelectedTrades(string pTrdSysCode, string pSeCptySn, string pCptySn, string pCdtyCode, 
            DateTime pBeginTradeDt, DateTime pEndTradeDt, string pTrdSysTicket, string pCptyTradeId, string pPermissionKeyInClause)
        {
            const string SELECT_OBJECT = "*";
            var result = new List<SummaryData>();
            Int32 tradeId = 0;
            string sql = GetAllTradeSummarySqlCommandStr(pTrdSysCode, pSeCptySn, pCptySn, pCdtyCode, 
                        pBeginTradeDt, pEndTradeDt, pTrdSysTicket, pCptyTradeId, SELECT_OBJECT);

            if (pPermissionKeyInClause != "")
                //Israel 11/20/2015 -- Added support for Null permission key
                //sql += " and " + pPermissionKeyInClause;
                sql += " and " + "( " + pPermissionKeyInClause + " or PERMISSION_KEY is null )";
            
            using (SqlConnection conn = new SqlConnection(sqlConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    conn.Open();
                    using (SqlDataReader dataReader = cmd.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                result.Add(new SummaryData
                                {
                                    AdditionalConfirmSent = dataReader["ADDITIONAL_CONFIRM_SENT"].ToString(),
                                    AnalystName = dataReader["ANALYST_NAME"].ToString(),
                                    ArchiveFlag = dataReader["ARCHIVE_FLAG"].ToString(),
                                    BookingCoId = DBUtils.HandleInt32IfNull(dataReader["BOOKING_CO_ID"].ToString()),
                                    BookingCoSn = dataReader["BOOKING_CO_SN"].ToString(),
                                    //BookingCoSn = dataReader["SE_CPTY_SN"].ToString(),
                                    BkrDbUpd = DBUtils.HandleInt32IfNull(dataReader["BKR_DB_UPD"].ToString()),
                                    BkrMeth = dataReader["BKR_METH"].ToString(),
                                    BkrRqmt = dataReader["BKR_RQMT"].ToString(),
                                    BkrStatus = dataReader["BKR_STATUS"].ToString(),
                                    Book = dataReader["BOOK"].ToString(),
                                    BrokerId = DBUtils.HandleInt32IfNull(dataReader["BROKER_ID"].ToString()),
                                    BrokerLegalName = dataReader["BROKER_LEGAL_NAME"].ToString(),
                                    BrokerPrice = dataReader["BROKER_PRICE"].ToString(),
                                    BrokerSn = dataReader["BROKER_SN"].ToString(),
                                    BuySellInd = dataReader["BUY_SELL_IND"].ToString(),
                                    CdtyCode = dataReader["CDTY_CODE"].ToString(),
                                    CdtyGrpCode = dataReader["CDTY_GRP_CODE"].ToString(),
                                    Cmt = dataReader["CMT"].ToString(),
                                    //Comm = dataReader["COMM"].ToString(),
                                    CptyDbUpd = DBUtils.HandleInt32IfNull(dataReader["CPTY_DB_UPD"].ToString()),
                                    CptyId = DBUtils.HandleInt32IfNull(dataReader["CPTY_ID"].ToString()),
                                    CptyLegalName = dataReader["CPTY_LEGAL_NAME"].ToString(),
                                    //CptyLn = dataReader["CPTY_LN"].ToString(),
                                    CptyMeth = dataReader["CPTY_METH"].ToString(),
                                    CptyRqmt = dataReader["CPTY_RQMT"].ToString(),
                                    CptySn = dataReader["CPTY_SN"].ToString(),
                                    CptyStatus = dataReader["CPTY_STATUS"].ToString(),
                                    CptyTradeId = dataReader["CPTY_TRADE_ID"].ToString(),
                                    CurrentBusnDt = DBUtils.HandleDateTimeIfNull(dataReader["CURRENT_BUSN_DT"].ToString()),
                                    //EfsCptySn = dataReader["EFS_CPTY_SN"].ToString(),
                                    //EfsFlag = dataReader["EFS_FLAG"].ToString(),
                                    EndDt = DBUtils.HandleDateTimeIfNull(dataReader["END_DT"].ToString()),
                                    FinalApprovalFlag = dataReader["FINAL_APPROVAL_FLAG"].ToString(),
                                    FinalApprovalTimestampGmt = DBUtils.HandleDateTimeIfNull(dataReader["FINAL_APPROVAL_TIMESTAMP_GMT"].ToString()),
                                    GroupXref = dataReader["GROUP_XREF"].ToString(),
                                    HasProblemFlag = dataReader["HAS_PROBLEM_FLAG"].ToString(),
                                    Id = DBUtils.HandleInt32IfNull(dataReader["ID"].ToString()),
                                    InceptionDt = DBUtils.HandleDateTimeIfNull(dataReader["INCEPTION_DT"].ToString()),
                                    IsTestBook = dataReader["IS_TEST_BOOK"].ToString(),
                                    LastTrdEditTimestampGmt = DBUtils.HandleDateTimeIfNull(dataReader["LAST_TRD_EDIT_TIMESTAMP_GMT"].ToString()),
                                    LastUpdateTimestampGmt = DBUtils.HandleDateTimeIfNull(dataReader["LAST_UPDATE_TIMESTAMP_GMT"].ToString()),
                                    LocationSn = dataReader["LOCATION_SN"].ToString(),
                                    MigrateInd = dataReader["MIGRATE_IND"].ToString(),
                                    NoconfDbUpd = DBUtils.HandleInt32IfNull(dataReader["NOCONF_DB_UPD"].ToString()),
                                    NoconfMeth = dataReader["NOCONF_METH"].ToString(),
                                    NoconfRqmt = dataReader["NOCONF_RQMT"].ToString(),
                                    NoconfStatus = dataReader["NOCONF_STATUS"].ToString(),
                                    OpsDetActFlag = dataReader["OPS_DET_ACT_FLAG"].ToString(),
                                    OptnPremPrice = dataReader["OPTN_PREM_PRICE"].ToString(),
                                    OptnPutCallInd = dataReader["OPTN_PUT_CALL_IND"].ToString(),
                                    OptnStrikePrice = dataReader["OPTN_STRIKE_PRICE"].ToString(),
                                    //PayPrice = dataReader["PAY_PRICE"].ToString(),
                                    PermissionKey = dataReader["PERMISSION_KEY"].ToString(),
                                    PlAmt = dataReader["PL_AMT"].ToString(),
                                    PriceDesc = dataReader["PRICE_DESC"].ToString(),
                                    Priority = dataReader["PRIORITY"].ToString(),
                                    //Qty = DBUtils.HandleFloatIfNull(dataReader["QTY"].ToString()),
                                    QuantityDescription = dataReader["QTY_DESC"].ToString(),
                                    QtyTot = DBUtils.HandleFloatIfNull(dataReader["QTY_TOT"].ToString()),
                                    ReadyForFinalApprovalFlag = dataReader["READY_FOR_FINAL_APPROVAL_FLAG"].ToString(),
                                    //RecPrice = dataReader["REC_PRICE"].ToString(),
                                    RecentInd = DBUtils.HandleInt32IfNull(dataReader["RECENT_IND"].ToString()),
                                    RefSn = dataReader["REF_SN"].ToString(),
                                    RplyRdyToSndFlag = dataReader["RPLY_RDY_TO_SND_FLAG"].ToString(),
                                    //SeCptySn = dataReader["SE_CPTY_SN"].ToString(),
                                    SetcDbUpd = DBUtils.HandleInt32IfNull(dataReader["SETC_DB_UPD"].ToString()),
                                    SetcMeth = dataReader["SETC_METH"].ToString(),
                                    SetcRqmt = dataReader["SETC_RQMT"].ToString(),
                                    SetcStatus = dataReader["SETC_STATUS"].ToString(),
                                    StartDt = DBUtils.HandleDateTimeIfNull(dataReader["START_DT"].ToString()),
                                    SttlType = dataReader["STTL_TYPE"].ToString(),
                                    TradeDesc = dataReader["TRADE_DESC"].ToString(),
                                    TradeDt = DBUtils.HandleDateTimeIfNull(dataReader["TRADE_DT"].ToString()),
                                    TradeId = DBUtils.HandleInt32IfNull(dataReader["TRADE_ID"].ToString()),
                                    Trader = dataReader["TRADER"].ToString(),
                                    TradeStatCode = dataReader["TRADE_STAT_CODE"].ToString(),
                                    TradeTypeCode = dataReader["TRADE_TYPE_CODE"].ToString(),
                                    TransactionSeq = DBUtils.HandleInt32IfNull(dataReader["TRANSACTION_SEQ"].ToString()),
                                    TransportDesc = dataReader["TRANSPORT_DESC"].ToString(),
                                    TrdSysCode = dataReader["TRD_SYS_CODE"].ToString(),
                                    TradeSysTicket = dataReader["TRD_SYS_TICKET"].ToString(),
                                    //UomDurCode = dataReader["UOM_DUR_CODE"].ToString(),
                                    VerblDbUpd = DBUtils.HandleInt32IfNull(dataReader["VERBL_DB_UPD"].ToString()),
                                    VerblMeth = dataReader["VERBL_METH"].ToString(),
                                    VerblRqmt = dataReader["VERBL_RQMT"].ToString(),
                                    VerblStatus = dataReader["VERBL_STATUS"].ToString(),
                                    Version = DBUtils.HandleInt32IfNull(dataReader["VERSION"].ToString()),
                                    Xref = dataReader["XREF"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            return result;
        }

        private string GetAllTradeSummarySqlCommandStr(string pTrdSysCode, string pSeCptySn, string pCptySn, string pCdtyCode,
            DateTime pBeginTradeDt, DateTime pEndTradeDt, string pTrdSysTicket, string pCptyTradeId, string pSelectObject)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            bool isWhereUsedYet = false;
            string statementPrefix = String.Empty;
            string sql = "select " + pSelectObject + " from " + DBUtils.SCHEMA_NAME + "v_pc_trade_summary ";
            sqlBuilder.Append(sql);

            if (pTrdSysTicket.Contains("*"))
            {
                int end = pTrdSysTicket.IndexOf("*");
                sql = " where trd_sys_ticket like '" + pTrdSysTicket.Substring(0, end).ToUpper() + "%' ";
                sqlBuilder.Append(sql);
            }
            else if (!String.IsNullOrEmpty(pTrdSysTicket))
            {
                sql = " where trd_sys_ticket = '" + pTrdSysTicket + "' ";
                sqlBuilder.Append(sql);
            }
            else if (!String.IsNullOrEmpty(pCptyTradeId))
            {
                sql = " where cpty_trade_id = '" + pCptyTradeId + "' ";
                sqlBuilder.Append(sql);
            }
            else
            {
                if (!String.IsNullOrEmpty(pTrdSysCode))
                {
                    statementPrefix = isWhereUsedYet ? "and" : "where";
                    isWhereUsedYet = true;
                    sql = statementPrefix + " trd_sys_code = '" + pTrdSysCode + "' ";
                    sqlBuilder.Append(sql);
                }
                if (!String.IsNullOrEmpty(pSeCptySn))
                {
                    statementPrefix = isWhereUsedYet ? "and" : "where";
                    isWhereUsedYet = true;
                    sql = statementPrefix + " se_cpty_sn = '" + pSeCptySn + "' ";
                    sqlBuilder.Append(sql);
                }
                if (!String.IsNullOrEmpty(pCptySn))
                {
                    statementPrefix = isWhereUsedYet ? "and" : "where";
                    isWhereUsedYet = true;
                    sql = statementPrefix + " cpty_sn = '" + pCptySn + "' ";
                    sqlBuilder.Append(sql);
                }
                if (!String.IsNullOrEmpty(pCdtyCode))
                {
                    statementPrefix = isWhereUsedYet ? "and" : "where";
                    isWhereUsedYet = true;
                    sql = statementPrefix + " cdty_code = '" + pCdtyCode + "' ";
                    sqlBuilder.Append(sql);
                }
                if (pBeginTradeDt > DateTime.MinValue && pEndTradeDt > DateTime.MinValue)
                {
                    statementPrefix = isWhereUsedYet ? "and" : "where";
                    isWhereUsedYet = true;
                    string beginDtStr = pBeginTradeDt.ToString("yyyy-MM-dd");
                    string endDtStr = pEndTradeDt.ToString("yyyy-MM-dd");

                    sql = statementPrefix + " trade_dt >= '" + beginDtStr + "' and " +
                          " and trade_dt <= '" + endDtStr + "' ";
                    sqlBuilder.Append(sql);
                }
            }

            return sqlBuilder.ToString();
        }

        public bool IsValidTradeId(Int32 pTradeId)
        {
            int result = 0;
            string sql = "select count(*) cnt from " + DBUtils.SCHEMA_NAME + "v_pc_trade_summary " +
                         "where trade_id = @trade_id";

            using (SqlConnection conn = new SqlConnection(sqlConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.Add("@trade_id", System.Data.SqlDbType.Int).Value = pTradeId;
                    conn.Open();
                    using (SqlDataReader dataReader = cmd.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                result = dataReader.GetInt32(dataReader.GetOrdinal("cnt"));
                            }
                        }
                    }
                }
            }
            return result > 0;
        }

        public string GetTradeIdListString(IList<SummaryData> pTradeList)
        {
            string tradeIdList = "(";
            bool addComma = false;
            foreach (SummaryData dataRow in pTradeList)
            {
                if (addComma)
                    tradeIdList += ",";
                tradeIdList += dataRow.TradeId.ToString();
                addComma = true;
            }
            tradeIdList += ")";
            return tradeIdList;
        }



    }
}
