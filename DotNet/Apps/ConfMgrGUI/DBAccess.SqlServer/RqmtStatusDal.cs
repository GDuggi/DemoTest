using OpsTrackingModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DBAccess.SqlServer
{
    public class RqmtStatusDal : IRqmtStatusDal
    {
        private string sqlConnStr = "";

        public RqmtStatusDal(string pSqlConn)
        {
            sqlConnStr = pSqlConn;
        }

        #region Stubbed Data

        public List<RqmtStatusView> GetAllStub()
        {
            var result = new List<RqmtStatusView>();
            result.Add(new RqmtStatusView
            {   RqmtCode = "NOCNF",
                DisplayText = "No Cpty Confirm",
                InitialStatus = "OPEN",
                StatusCode = "APPR",
                Ord = 2,
                TerminalFlag = "Y",
                ProblemFlag = "N",  
                ColorCode = "LightGreen", 
                Descr = "Approved" } );
            result.Add(new RqmtStatusView
            {
                RqmtCode = "NOCNF",
                DisplayText = "No Cpty Confirm",
                InitialStatus = "OPEN",
                StatusCode = "CXL",
                Ord = 99,
                TerminalFlag = "Y",
                ProblemFlag = "N",
                ColorCode = "LightGray",
                Descr = "Canceled"
            });
            result.Add(new RqmtStatusView
            {
                RqmtCode = "NOCNF",
                DisplayText = "No Cpty Confirm",
                InitialStatus = "OPEN",
                StatusCode = "OPEN",
                Ord = 0,
                TerminalFlag = "N",
                ProblemFlag = "N",
                ColorCode = "SkyBlue",
                Descr = "Open"
            });
            result.Add(new RqmtStatusView
            {
                RqmtCode = "XQCSP",
                DisplayText = "Our Paper",
                InitialStatus = "NEW",
                StatusCode = "MGR",
                Ord = 5,
                TerminalFlag = "N",
                ProblemFlag = "N",
                ColorCode = "Gold",
                Descr = "Review by Manager"
            });
            result.Add(new RqmtStatusView
            {
                RqmtCode = "XQCSP",
                DisplayText = "Our Paper",
                InitialStatus = "NEW",
                StatusCode = "OK_TO_SEND",
                Ord = 6,
                TerminalFlag = "N",
                ProblemFlag = "N",
                ColorCode = "Gold",
                Descr = "Contract Approved To Send"
            });
            result.Add(new RqmtStatusView
            {
                RqmtCode = "XQCSP",
                DisplayText = "Our Paper",
                InitialStatus = "NEW",
                StatusCode = "NEW",
                Ord = 0,
                TerminalFlag = "N",
                ProblemFlag = "N",
                ColorCode = "SkyBlue",
                Descr = "Template Not Assigned"
            });
            result.Add(new RqmtStatusView
            {
                RqmtCode = "XQCSP",
                DisplayText = "Our Paper",
                InitialStatus = "NEW",
                StatusCode = "SENT",
                Ord = 7,
                TerminalFlag = "N",
                ProblemFlag = "N",
                ColorCode = "Gold",
                Descr = "Sent"
            });
            result.Add(new RqmtStatusView
            {
                RqmtCode = "XQCSP",
                DisplayText = "Our Paper",
                InitialStatus = "NEW",
                StatusCode = "DISP",
                Ord = 10,
                TerminalFlag = "N",
                ProblemFlag = "Y",
                ColorCode = "Tomato",
                Descr = "Disputed"
            });
            result.Add(new RqmtStatusView
            {
                RqmtCode = "XQCSP",
                DisplayText = "Our Paper",
                InitialStatus = "NEW",
                StatusCode = "APPR",
                Ord = 13,
                TerminalFlag = "Y",
                ProblemFlag = "N",
                ColorCode = "LightGreen",
                Descr = "Approved"
            });
            result.Add(new RqmtStatusView
            {
                RqmtCode = "XQCSP",
                DisplayText = "Our Paper",
                InitialStatus = "NEW",
                StatusCode = "CXL",
                Ord = 99,
                TerminalFlag = "Y",
                ProblemFlag = "N",
                ColorCode = "LightGray",
                Descr = "Canceled"
            });
            result.Add(new RqmtStatusView
            {
                RqmtCode = "XQCSP",
                DisplayText = "Our Paper",
                InitialStatus = "NEW",
                StatusCode = "PREP",
                Ord = 1,
                TerminalFlag = "N",
                ProblemFlag = "N",
                ColorCode = "SandyBrown",
                Descr = "Beign Prepared"
            });
            result.Add(new RqmtStatusView
            {
                RqmtCode = "XQCSP",
                DisplayText = "Our Paper",
                InitialStatus = "NEW",
                StatusCode = "FAIL",
                Ord = 9,
                TerminalFlag = "N",
                ProblemFlag = "Y",
                ColorCode = "Tomato",
                Descr = "Transmission Failed"
            });
            result.Add(new RqmtStatusView
            {
                RqmtCode = "XQCSP",
                DisplayText = "Our Paper",
                InitialStatus = "NEW",
                StatusCode = "VERBL",
                Ord = 17,
                TerminalFlag = "Y",
                ProblemFlag = "N",
                ColorCode = "LightGreen",
                Descr = "Called Cpty"
            });
            result.Add(new RqmtStatusView
            {
                RqmtCode = "XQCSP",
                DisplayText = "Our Paper",
                InitialStatus = "NEW",
                StatusCode = "CPRCV",
                Ord = 16,
                TerminalFlag = "Y",
                ProblemFlag = "N",
                ColorCode = "LightGreen",
                Descr = "Recvd Cpty Paper"
            });
            result.Add(new RqmtStatusView
            {
                RqmtCode = "XQCSP",
                DisplayText = "Our Paper",
                InitialStatus = "NEW",
                StatusCode = "PSTVL",
                Ord = 15,
                TerminalFlag = "Y",
                ProblemFlag = "N",
                ColorCode = "LightGreen",
                Descr = "Trade Past Value"
            });
            result.Add(new RqmtStatusView
            {
                RqmtCode = "XQCSP",
                DisplayText = "Our Paper",
                InitialStatus = "NEW",
                StatusCode = "SIGNED",
                Ord = 14,
                TerminalFlag = "Y",
                ProblemFlag = "N",
                ColorCode = "LightGreen",
                Descr = "Signed Paper"
            });
            result.Add(new RqmtStatusView
            {
                RqmtCode = "XQCSP",
                DisplayText = "Our Paper",
                InitialStatus = "NEW",
                StatusCode = "RESNT",
                Ord = 11,
                TerminalFlag = "N",
                ProblemFlag = "Y",
                ColorCode = "Tomato",
                Descr = "Resent Our Paper"
            });
            result.Add(new RqmtStatusView
            {
                RqmtCode = "XQCSP",
                DisplayText = "Our Paper",
                InitialStatus = "NEW",
                StatusCode = "TRADER",
                Ord = 4,
                TerminalFlag = "N",
                ProblemFlag = "N",
                ColorCode = "Gold",
                Descr = "Review By Trader"
            });
            result.Add(new RqmtStatusView
            {
                RqmtCode = "XQCSP",
                DisplayText = "Our Paper",
                InitialStatus = "NEW",
                StatusCode = "EXT_REVIEW",
                Ord = 3,
                TerminalFlag = "N",
                ProblemFlag = "N",
                ColorCode = "Gold",
                Descr = "Credit Hold"
            });
            result.Add(new RqmtStatusView
            {
                RqmtCode = "XQCSP",
                DisplayText = "Our Paper",
                InitialStatus = "NEW",
                StatusCode = "AUTO",
                Ord = 8,
                TerminalFlag = "N",
                ProblemFlag = "N",
                ColorCode = "Gold",
                Descr = "Auto Match Returned Doc"
            });
            result.Add(new RqmtStatusView
            {
                RqmtCode = "XQCSP",
                DisplayText = "Our Paper",
                InitialStatus = "NEW",
                StatusCode = "ACPTD",
                Ord = 18,
                TerminalFlag = "Y",
                ProblemFlag = "N",
                ColorCode = "LightGreen",
                Descr = "Deemed Accepted"
            });
            return result;
        }

        #endregion

        public List<RqmtStatusView> GetAll()
        {
            var result = new List<RqmtStatusView>();
            string sql = "select rqmt_code, display_text, initial_status, status_code, ord, " + 
                         "terminal_flag, problem_flag, color_code, descr " +
                         "from " + DBUtils.SCHEMA_NAME + "v_rqmt_status ";

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
                                result.Add(new RqmtStatusView
                                {
                                    RqmtCode = dataReader["rqmt_code"].ToString(),
                                    DisplayText = dataReader["display_text"].ToString(),
                                    InitialStatus = dataReader["initial_status"].ToString(),
                                    StatusCode = dataReader["status_code"].ToString(),
                                    Ord = DBUtils.HandleInt32IfNull(dataReader["ord"].ToString()),
                                    TerminalFlag = dataReader["terminal_flag"].ToString(),
                                    ProblemFlag = dataReader["problem_flag"].ToString(),
                                    ColorCode = dataReader["color_code"].ToString(),
                                    Descr = dataReader["descr"].ToString()
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
