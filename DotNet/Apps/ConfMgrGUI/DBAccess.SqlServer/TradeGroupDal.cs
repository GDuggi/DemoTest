using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DBAccess.SqlServer
{
    public class TradeGroupDal : ITradeGroupDal
    {
        public const string SEQ_NAME = "seq_trade_group";
        private const string PROJ_FILE_NAME = "TradeGroupDal";
        private string sqlConnStr = "";

        public TradeGroupDal(string pSqlConn)
        {
            sqlConnStr = pSqlConn;
        }

        public Int32 CleanTestData(string pXref)
        {
            string sql = "delete from " + DBUtils.SCHEMA_NAME + "trade_group where xref = @xref";
            Int32 rowsDeleted = 0;

            using (SqlConnection conn = new SqlConnection(sqlConnStr))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(sql, conn, trans))
                        {
                            cmd.CommandType = System.Data.CommandType.Text;
                            cmd.Parameters.Add("@xref", System.Data.SqlDbType.VarChar).Value = pXref;
                            rowsDeleted = cmd.ExecuteNonQuery();
                        }
                        trans.Commit();
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        throw new Exception("An error occurred while updating the database." + Environment.NewLine +
                            "Error CNF-365 in " + PROJ_FILE_NAME + ".CleanTestData(): " + e.Message);
                    }
                }
            }
            return rowsDeleted;
        }

        public Int32 Group(List<TradeGroupDto> pDataList)
        {
            Int32 rowsInserted = 0;
            Int32 rowIns = 0;
            Int32 tradeGroupId = 0;
            string sql = "Insert into " + DBUtils.SCHEMA_NAME + "trade_group " +
                    "   (id, trade_id, xref) " +
                    " Values " +
                    "   (@id, @trade_id, @xref) ";

            using (SqlConnection conn = new SqlConnection(sqlConnStr))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    using (SqlCommand cmd = new SqlCommand(sql, conn, trans))
                    {
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.Parameters.Add("@id", System.Data.SqlDbType.Int);
                        cmd.Parameters.Add("@trade_id", System.Data.SqlDbType.Int);
                        cmd.Parameters.Add("@xref", System.Data.SqlDbType.VarChar);

                        try
                        {                           
                            foreach (TradeGroupDto tradeGroupItem in pDataList)
                            {
                                tradeGroupId = DBUtils.GetNextSequence(sqlConnStr, SEQ_NAME);
                                cmd.Parameters["@id"].Value = tradeGroupId;
                                cmd.Parameters["@trade_id"].Value = tradeGroupItem.TradeId;
                                cmd.Parameters["@xref"].Value = tradeGroupItem.Xref;
                                rowIns = cmd.ExecuteNonQuery();

                                //rowIns return value is erratic. This approach produces reliable results.
                                rowIns = rowIns > 0 ? 1 : 0;
                                rowsInserted += rowIns;
                            }
                            trans.Commit();
                        }
                        catch (Exception e)
                        {
                            trans.Rollback();
                            throw new Exception("An error occurred while updating the database." + Environment.NewLine +
                                "Error CNF-366 in " + PROJ_FILE_NAME + ".Group(): " + e.Message);
                        }
                    }
                }
            }
            return rowsInserted;
        }

        public Int32 Ungroup(List<Int32> pTradeIdList)
        {
            string tradeIds = String.Join(",", pTradeIdList);
            string sql = "delete from " + DBUtils.SCHEMA_NAME + "trade_group where trade_id = @trade_id";
            Int32 rowsDeleted = 0;
            Int32 rowDel = 0;

            using (SqlConnection conn = new SqlConnection(sqlConnStr))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    using (SqlCommand cmd = new SqlCommand(sql, conn, trans))
                    {
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.Parameters.Add("@trade_id", System.Data.SqlDbType.Int);

                        try
                        {
                            foreach (Int32 tradeId in pTradeIdList)
                            {
                                cmd.Parameters["@trade_id"].Value = tradeId;
                                rowDel = cmd.ExecuteNonQuery();

                                //rowDel return value is erratic. This approach produces reliable results.
                                rowDel = rowDel > 0 ? 1 : 0;
                                rowsDeleted += rowDel;
                            }
                            trans.Commit();
                        }
                        catch (Exception e)
                        {
                            trans.Rollback();
                            throw new Exception("An error occurred while updating the database." + Environment.NewLine +
                                "Error CNF-367 in " + PROJ_FILE_NAME + ".Ungroup(): " + e.Message);
                        }
                    }
                }
            }
            return rowsDeleted;
        }

    }
}
