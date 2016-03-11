using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OracleClient;
using System.Data;

namespace CommonUtils
{
    public class DataAccess : IDisposable
    {
        public OracleConnection conn;
        private bool disposed;
        public DataAccess(string connectionString)
        {
            conn = new OracleConnection(connectionString);
            conn.Open();
            disposed = false;
        }
        #region IDisposable Members
        public void Dispose()
        {
            if (!disposed)
            {
                if (conn != null && conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                    conn = null;
                }
                disposed = true;
            }
        }
        #endregion
    }
}
