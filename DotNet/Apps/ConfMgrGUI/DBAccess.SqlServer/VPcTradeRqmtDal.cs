using OpsTrackingModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;

namespace DBAccess.SqlServer
{
    public class VPcTradeRqmtDal : IVPcTradeRqmtDal
    {
        private string sqlConnStr = "";

        public VPcTradeRqmtDal(string pSqlConn)
        {
            sqlConnStr = pSqlConn;
        }

        #region Stubbed Data

        public List<RqmtData> GetAllStub()
        {
            //string dateStr = "";
            var result = new List<RqmtData>();
            result.Add(new RqmtData
            {
                Id = 270328,
                TradeId = 1483078,
                RqmtTradeNotifyId = 243907,
                Rqmt = "NOCNF",
                Status = "APPR",
                CompletedDt = DateTime.ParseExact("03-25-2015", "MM-dd-yyyy", CultureInfo.InvariantCulture),
                Reference = "OTHER",
                SecondCheckFlag = "N",
                TransactionSeq = 1003766,
                FinalApprovalFlag = "N",
                DisplayText = "No Cpty Confirm",
                Category = "OPS",
                TerminalFlag = "Y",
                ProblemFlag = "N",
                GuiColorCode = "GREEN",
                DelphiConstant = "$00B9F0BE"
            });

            result.Add(new RqmtData
            {
                Id = 279970,
                TradeId = 1493829,
                RqmtTradeNotifyId = 243907,
                Rqmt = "XQCSP",
                Status = "SENT",
                CompletedDt = DateTime.ParseExact("04-13-2015", "MM-dd-yyyy", CultureInfo.InvariantCulture),
                Reference = "OTHER",
                SecondCheckFlag = "N",
                TransactionSeq = 1025915,
                FinalApprovalFlag = "N",
                DisplayText = "Our Paper",
                Category = "OURC",
                TerminalFlag = "N",
                ProblemFlag = "N",
                GuiColorCode = "YELLOW",
                DelphiConstant = "$0080FFFF"
            });

            result.Add(new RqmtData
            {
                Id = 280559,
                TradeId = 1495866,
                RqmtTradeNotifyId = 250979,
                Rqmt = "NOCNF",
                Status = "APPR",
                CompletedDt = DateTime.ParseExact("04-28-2015", "MM-dd-yyyy", CultureInfo.InvariantCulture),
                Reference = "CPEXC",
                Cmt = "Assoc doc",
                SecondCheckFlag = "N",
                TransactionSeq = 1026354,
                FinalApprovalFlag = "N",
                DisplayText = "No Cpty Confirm",
                Category = "OPS",
                TerminalFlag = "Y",
                ProblemFlag = "N",
                GuiColorCode = "GREEN",
                DelphiConstant = "$00B9F0BE"
            });

            return result;
        }

        #endregion

        public List<RqmtData> GetAll()
        {
            var result = new List<RqmtData>();
            string sql = "select * from " + DBUtils.SCHEMA_NAME + "v_pc_trade_rqmt " +
                        " where final_approval_flag = 'N' " +
                        " order by TRANSACTION_SEQ ASC";

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
                                result.Add(new RqmtData
                                {
                                    CancelTradeNotifyId = DBUtils.HandleInt32IfNull(dataReader["CANCEL_TRADE_NOTIFY_ID"].ToString()),
                                    Category = dataReader["CATEGORY"].ToString(),
                                    Cmt = dataReader["CMT"].ToString(),
                                    TrdSysTicket = dataReader["TRD_SYS_TICKET"].ToString(),
                                    TrdSysCode = dataReader["TRD_SYS_CODE"].ToString(),
                                    CompletedDt = DBUtils.HandleDateTimeIfNull(dataReader["COMPLETED_DT"].ToString()),
                                    CompletedTimestampGmt = DBUtils.HandleDateTimeIfNull(dataReader["COMPLETED_TIMESTAMP_GMT"].ToString()),
                                    DelphiConstant = dataReader["DELPHI_CONSTANT"].ToString(),
                                    DisplayText = dataReader["DISPLAY_TEXT"].ToString(),
                                    FinalApprovalFlag = dataReader["FINAL_APPROVAL_FLAG"].ToString(),
                                    GuiColorCode = dataReader["GUI_COLOR_CODE"].ToString(),
                                    Id = DBUtils.HandleInt32IfNull(dataReader["ID"].ToString()),
                                    //PrelimAppr = dataReader["PRELIM_APPR"].ToString(),
                                    ProblemFlag = dataReader["PROBLEM_FLAG"].ToString(),
                                    Reference = dataReader["REFERENCE"].ToString(),
                                    Rqmt = dataReader["RQMT"].ToString(),
                                    RqmtTradeNotifyId = DBUtils.HandleInt32IfNull(dataReader["RQMT_TRADE_NOTIFY_ID"].ToString()),
                                    SecondCheckFlag = dataReader["SECOND_CHECK_FLAG"].ToString(),
                                    Status = dataReader["STATUS"].ToString(),
                                    TerminalFlag = dataReader["TERMINAL_FLAG"].ToString(),
                                    TradeId = DBUtils.HandleInt32IfNull(dataReader["TRADE_ID"].ToString()),
                                    TransactionSeq = DBUtils.HandleInt32IfNull(dataReader["TRANSACTION_SEQ"].ToString())
                                });
                            }
                        }
                    }
                }
            }
            return result;
        }

        public List<RqmtData> GetAll(string pTradeIdList)
        {
            var result = new List<RqmtData>();
            string sql = "select * from " + DBUtils.SCHEMA_NAME + "v_pc_trade_rqmt " +
                "where trade_id in " + pTradeIdList;

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
                                result.Add(new RqmtData
                                {
                                    CancelTradeNotifyId = DBUtils.HandleInt32IfNull(dataReader["CANCEL_TRADE_NOTIFY_ID"].ToString()),
                                    Category = dataReader["CATEGORY"].ToString(),
                                    Cmt = dataReader["CMT"].ToString(),
                                    TrdSysTicket = dataReader["TRD_SYS_TICKET"].ToString(),
                                    TrdSysCode = dataReader["TRD_SYS_CODE"].ToString(),
                                    CompletedDt = DBUtils.HandleDateTimeIfNull(dataReader["COMPLETED_DT"].ToString()),
                                    CompletedTimestampGmt = DBUtils.HandleDateTimeIfNull(dataReader["COMPLETED_TIMESTAMP_GMT"].ToString()),
                                    DelphiConstant = dataReader["DELPHI_CONSTANT"].ToString(),
                                    DisplayText = dataReader["DISPLAY_TEXT"].ToString(),
                                    FinalApprovalFlag = dataReader["FINAL_APPROVAL_FLAG"].ToString(),
                                    GuiColorCode = dataReader["GUI_COLOR_CODE"].ToString(),
                                    Id = DBUtils.HandleInt32IfNull(dataReader["ID"].ToString()),
                                    //PrelimAppr = dataReader["PRELIM_APPR"].ToString(),
                                    ProblemFlag = dataReader["PROBLEM_FLAG"].ToString(),
                                    Reference = dataReader["REFERENCE"].ToString(),
                                    Rqmt = dataReader["RQMT"].ToString(),
                                    RqmtTradeNotifyId = DBUtils.HandleInt32IfNull(dataReader["RQMT_TRADE_NOTIFY_ID"].ToString()),
                                    SecondCheckFlag = dataReader["SECOND_CHECK_FLAG"].ToString(),
                                    Status = dataReader["STATUS"].ToString(),
                                    TerminalFlag = dataReader["TERMINAL_FLAG"].ToString(),
                                    TradeId = DBUtils.HandleInt32IfNull(dataReader["TRADE_ID"].ToString()),
                                    TransactionSeq = DBUtils.HandleInt32IfNull(dataReader["TRANSACTION_SEQ"].ToString())                                    
                                });
                            }
                        }
                    }
                }
            }
            return result;
        }

    }
}
