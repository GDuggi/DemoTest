using System;
using System.Data;
using System.Data.SqlClient;

namespace DBAccess.SqlServer
{
    public class XmitRequestDal : IXmitRequestDal
    {
        private readonly String connectionString;

        public XmitRequestDal(string connectionStr)
        {
            connectionString = connectionStr;
        }

        public int SaveAssociatedDocumentXmitRequest(int id, TransmitDestinationType destinationType, string destination,
            string userName)
        {
            return CallInsertVaultRequest(destination, destinationType, userName,
                cmd => { cmd.Parameters.AddWithValue("@p_associated_docs_id", id); });
        }

        public int SaveTradeRqmtConfirmXmitRequest(int id, TransmitDestinationType destinationType, string destination,
            string userName)
        {
            return CallInsertVaultRequest(destination, destinationType, userName,
                cmd => { cmd.Parameters.AddWithValue("@p_trade_rqmt_confirm_id", id); });
        }

        private int CallInsertVaultRequest(string destination,
            TransmitDestinationType type,
            string userName,
            Action<SqlCommand> setParametersAction)
        {
            var methodIndicator = GetMethodIndicator(type);
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd =
                    new SqlCommand(string.Format("{0}P_INSERT_XMIT_REQUEST", DBUtils.SCHEMA_NAME), conn))
                {
                    setParametersAction(cmd);
                    cmd.Parameters.AddWithValue("@p_xmit_method_ind", methodIndicator);
                    cmd.Parameters.AddWithValue("@p_xmit_dest", destination);
                    cmd.Parameters.AddWithValue("@p_sent_by_user", userName);
                    cmd.CommandType = CommandType.StoredProcedure;
                    var ret = cmd.ExecuteScalar();
                    return (int) ret;
                }
            }
        }

        private static string GetMethodIndicator(TransmitDestinationType type)
        {
            return type.ToString().Substring(0, 1);
        }
    }
}