using OpsTrackingModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DBAccess.SqlServer
{
    public class RqmtDal : IRqmtDal
    {
        private string sqlConnStr = "";

        public RqmtDal(string pSqlConn)
        {
            sqlConnStr = pSqlConn;
        }

        #region Stubbed Data

        public List<RqmtView> GetAllStub()
        {
            var result = new List<RqmtView>();
            result.Add(new RqmtView
            {
                Code = "ECBKR",
                Descr = "EXECUTE ECONFIRM BROKER",
                Category = "BKR",
                InitialStatus = "PREP",
                DisplayText = "eConfirm Broker",
                ActiveFlag = "Y",
                DetActRqmtFlag = "N"
            });
            result.Add(new RqmtView
            {
                Code = "ECONF",
                Descr = "EXECUTE ECONFIRM",
                Category = "OURC",
                InitialStatus = "PREP",
                DisplayText = "eConfirm Cpty",
                ActiveFlag = "Y",
                DetActRqmtFlag = "N"
            });
            result.Add(new RqmtView
            {
                Code = "EFBKR",
                Descr = "EXECUTE BROKER EFET",
                Category = "BLR",
                InitialStatus = "QUEUE",
                DisplayText = "EFET Broker",
                ActiveFlag = "Y",
                DetActRqmtFlag = "N"
            });
            result.Add(new RqmtView
            {
                Code = "EFET",
                Descr = "EXECUTE EFET",
                Category = "OURC",
                InitialStatus = "QUEUE",
                DisplayText = "EFET Cpty",
                ActiveFlag = "Y",
                DetActRqmtFlag = "N"
            });
            result.Add(new RqmtView
            {
                Code = "NOCNF",
                Descr = "NO CONFIRM APPROVAL",
                Category = "OPS",
                InitialStatus = "OPEN",
                DisplayText = "No Cpty Confirm",
                ActiveFlag = "Y",
                DetActRqmtFlag = "Y"
            });
            result.Add(new RqmtView
            {
                Code = "VBCP",
                Descr = "VERBAL CONFIRM",
                Category = "VERBL",
                InitialStatus = "PEND",
                DisplayText = "Phone",
                ActiveFlag = "Y",
                DetActRqmtFlag = "Y"
            });
            result.Add(new RqmtView
            {
                Code = "XQBBP",
                Descr = "EXECUTE BROKER CONFIRM - BROKER'S PAPER",
                Category = "BKR",
                InitialStatus = "EXPCT",
                DisplayText = "Broker Paper",
                ActiveFlag = "Y",
                DetActRqmtFlag = "Y"
            });
            result.Add(new RqmtView
            {
                Code = "XQCCP",
                Descr = "EXECUTE CPTY CONTRACT - CPTY'S PAPER",
                Category = "CPTY",
                InitialStatus = "EXPCT",
                DisplayText = "Cpty Paper",
                ActiveFlag = "Y",
                DetActRqmtFlag = "Y"
            });
            result.Add(new RqmtView
            {
                Code = "XQCSP",
                Descr = "EXECUTE CPTY CONTRACT - OUR PAPER",
                Category = "OURC",
                InitialStatus = "NEW",
                DisplayText = "Our Paper",
                ActiveFlag = "Y",
                DetActRqmtFlag = "Y"
            });
            return result;
        }

        #endregion

        public List<RqmtView> GetAll()
        {
            var result = new List<RqmtView>();
            string sql = "select code, descr, category, initial_status, display_text, active_flag, det_act_rqmt_flag " +
                         "from " + DBUtils.SCHEMA_NAME + "v_rqmt where active_flag = 'Y' ";

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
                                result.Add(new RqmtView
                                {
                                    Code = dataReader["code"].ToString(),
                                    Descr = dataReader["descr"].ToString(),
                                    Category = dataReader["category"].ToString(),
                                    InitialStatus = dataReader["initial_status"].ToString(),
                                    DisplayText = dataReader["display_text"].ToString(),
                                    ActiveFlag = dataReader["active_flag"].ToString(),
                                    DetActRqmtFlag = dataReader["det_act_rqmt_flag"].ToString()
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
