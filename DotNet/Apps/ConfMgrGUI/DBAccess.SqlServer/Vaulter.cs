using System;
using System.Data;
using System.Data.SqlClient;

namespace DBAccess.SqlServer
{
    public class Vaulter : IVaulter
    {
        private readonly string sqlConnStr;

        public Vaulter(string sqlConnStr)
        {
            this.sqlConnStr = sqlConnStr;
        }

        public void VaultAssociatedDoc(int docId, string metaData)
        {
            CallInsertVaultRequest(metaData, 
                cmd =>
                {
                    cmd.Parameters.AddWithValue("@p_trade_rqmt_confirm_id", DBNull.Value);
                    cmd.Parameters.AddWithValue("@p_associated_docs_id", docId);
                    cmd.Parameters.AddWithValue("@p_sent_flag", "Y");
                } 
            );
        }

        public void VaultTradeRqmtConfirm(int confirmId, string metaData)
        {
            CallInsertVaultRequest(metaData,
                cmd =>
                {
                    cmd.Parameters.AddWithValue("@p_trade_rqmt_confirm_id", confirmId);
                    cmd.Parameters.AddWithValue("@p_associated_docs_id", DBNull.Value);
                    cmd.Parameters.AddWithValue("@p_sent_flag", "Y");
                }
            );
        }

        private void CallInsertVaultRequest(string metaData, Action<SqlCommand> setParametersAction)
        {
            using (var conn = new SqlConnection(sqlConnStr))
            {
                conn.Open();
                using (var cmd =
                    new SqlCommand(string.Format("{0}P_INSERT_VAULT_REQUEST", DBUtils.SCHEMA_NAME), conn))
                {
                    setParametersAction(cmd);
                    cmd.Parameters.AddWithValue("@p_metadata", (object)metaData ?? DBNull.Value);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
