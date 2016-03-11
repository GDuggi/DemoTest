using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;

namespace DBAccess.SqlServer
{
    public class TradeRqmtConfirmBlobDal : ITradeRqmtConfirmBlobDal
    {

        public const string SEQ_NAME = "seq_trade_rqmt_confirm_blob";
        private string sqlConnStr = "";

        public TradeRqmtConfirmBlobDal(string pSqlConn)
        {
            sqlConnStr = pSqlConn;
        }

        private void SaveDocImageFile(string pFileNameAndPath, string pServerPath, byte[] pServerTxn)
        {
            const int BlockSize = 1024 * 512;
            using (FileStream source = new FileStream(pFileNameAndPath, FileMode.Open, FileAccess.Read))
            {
                using (SqlFileStream dest = new SqlFileStream(pServerPath, pServerTxn, FileAccess.Write))
                {
                    byte[] buffer = new byte[BlockSize];
                    int bytesRead;
                    while ((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        dest.Write(buffer, 0, bytesRead);
                        dest.Flush();
                    }
                    dest.Close();
                }
                source.Close();
            }
        }

        private void SaveDocImageByteArray(byte[] pDataBytes, string pServerPath, byte[] pServerTxn)
        {
            const int BlockSize = 1024 * 512;
            using (MemoryStream memStream = new MemoryStream(pDataBytes))
            //using (FileStream source = new FileStream(pFileNameAndPath, FileMode.Open, FileAccess.Read))
            {
                using (SqlFileStream dest = new SqlFileStream(pServerPath, pServerTxn, FileAccess.Write))
                {
                    byte[] buffer = new byte[BlockSize];
                    int bytesRead;
                    while ((bytesRead = memStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        dest.Write(buffer, 0, bytesRead);
                        dest.Flush();
                    }
                    dest.Close();
                }
                //source.Close();
            }
        }

        #region Test Database Data Operations

        public void TestInsertDocImageByteArray(int pImageId, string pDesc, byte[] pDataBytes, string pImagetype)
        {
            const string InsertTSql = @"
                INSERT INTO InboundImages(ImageId, Description, ImageType)
                VALUES(@ImageId, @Description, @ImageType);
                SELECT DocImage.PathName(), GET_FILESTREAM_TRANSACTION_CONTEXT()
                FROM InboundImages
                WHERE ImageId = @ImageId";
            string serverPath;
            byte[] serverTxn;
            using (TransactionScope ts = new TransactionScope())
            {
                using (SqlConnection conn = new SqlConnection(sqlConnStr))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(InsertTSql, conn))
                    {
                        cmd.Parameters.Add("@ImageId", SqlDbType.Int).Value = pImageId;
                        cmd.Parameters.Add("@Description", SqlDbType.VarChar).Value = pDesc;
                        cmd.Parameters.Add("@ImageType", SqlDbType.VarChar).Value = pImagetype.ToUpper();
                        using (SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.SingleRow))
                        {
                            rdr.Read();
                            serverPath = rdr.GetSqlString(0).Value;
                            serverTxn = rdr.GetSqlBinary(1).Value;
                            rdr.Close();
                        }
                    }
                    SaveDocImageByteArray(pDataBytes, serverPath, serverTxn);
                }
                ts.Complete();
            }
        }


        public void TestUpdateInboundImages(Int32 pImageId, string pDesc, string pImageType, byte[] pImageDataBytes)
        {
            const string InsertTSql = @"
                UPDATE InboundImages
                SET Description = @Description, ImageType = @ImageType
                WHERE ImageId = @ImageId
                SELECT TOP(1) DocImage.PathName(), GET_FILESTREAM_TRANSACTION_CONTEXT() 
                FROM InboundImages
                WHERE ImageId = @ImageId";
            using (SqlConnection conn = new SqlConnection(sqlConnStr))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(InsertTSql, conn))
                {
                    SqlTransaction tran = conn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                    cmd.Transaction = tran;

                    cmd.Parameters.Add("@ImageId", SqlDbType.Int).Value = pImageId;
                    cmd.Parameters.Add("@Description", SqlDbType.VarChar).Value = pDesc;
                    cmd.Parameters.Add("@ImageType", SqlDbType.VarChar).Value = pImageType.ToUpper();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Get the pointer for file 
                            string path = reader.GetString(0);
                            byte[] transactionContext = reader.GetSqlBytes(1).Buffer;

                            // Create the SqlFileStream
                            using (Stream fileStream = new SqlFileStream(path, transactionContext, FileAccess.Write, FileOptions.SequentialScan, allocationSize: 0))
                            {
                                // Write a single byte to the file. This will
                                // replace any data in the file.
                                //fileStream.WriteByte(0x01);
                                fileStream.Write(pImageDataBytes, 0, pImageDataBytes.Length);
                            }
                        }
                    }
                    tran.Commit();
                }
            }
        }

        public void TestOverwriteFilestream(Int32 pImageId, byte[] pImageDataBytes)
        {
            const string InsertTSql = @"
                SELECT TOP(1) DocImage.PathName(), GET_FILESTREAM_TRANSACTION_CONTEXT() 
                FROM InboundImages
                WHERE ImageId = @ImageId";
            using (SqlConnection conn = new SqlConnection(sqlConnStr))
            {
                conn.Open();

                //SqlCommand command = new SqlCommand("SELECT TOP(1) Photo.PathName(), GET_FILESTREAM_TRANSACTION_CONTEXT() FROM employees", connection);
                //SqlCommand command = new SqlCommand(InsertTSql, conn);
                using (SqlCommand cmd = new SqlCommand(InsertTSql, conn))
                {
                    SqlTransaction tran = conn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                    cmd.Transaction = tran;

                    cmd.Parameters.Add("@ImageId", SqlDbType.Int).Value = pImageId;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Get the pointer for file 
                            string path = reader.GetString(0);
                            byte[] transactionContext = reader.GetSqlBytes(1).Buffer;

                            // Create the SqlFileStream
                            using (Stream fileStream = new SqlFileStream(path, transactionContext, FileAccess.Write, FileOptions.SequentialScan, allocationSize: 0))
                            {
                                // Write a single byte to the file. This will
                                // replace any data in the file.
                                //fileStream.WriteByte(0x01);
                                fileStream.Write(pImageDataBytes, 0, pImageDataBytes.Length);
                            }
                        }
                    }
                    tran.Commit();
                }
            }
        }

        public byte[] TestGetByteArray(int pImageId)
        {
            const string SelectTSql = @"
                SELECT
                Description, ImageType, DocImage.PathName(), GET_FILESTREAM_TRANSACTION_CONTEXT()
                FROM InboundImages
                WHERE ImageId = @ImageId";
            //Image docImage;
            string serverPath;
            byte[] serverTxn;
            byte[] returnVal;
            using (TransactionScope ts = new TransactionScope())
            {
                using (SqlConnection conn = new SqlConnection(sqlConnStr))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(SelectTSql, conn))
                    {
                        cmd.Parameters.Add("@ImageId", SqlDbType.Int).Value = pImageId;
                        using (SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.SingleRow))
                        {
                            rdr.Read();
                            //desc = rdr.GetSqlString(0).Value;
                            //fileext = rdr.GetSqlString(1).Value;
                            serverPath = rdr.GetSqlString(2).Value;
                            serverTxn = rdr.GetSqlBinary(3).Value;
                            rdr.Close();
                        }
                    }

                    using (SqlFileStream sfs = new SqlFileStream(serverPath, serverTxn, FileAccess.Read))
                    {
                        using (MemoryStream memStream = new MemoryStream())
                        {
                            sfs.CopyTo(memStream);
                            returnVal = memStream.ToArray();
                        }
                        sfs.Close();
                    } 
                }
                ts.Complete();
            }
            return returnVal;
        }

        #endregion

        public Int32 GetCount(Int32 pTradeRqmtConfirmId)
        {
            Int32 result = 0;
            string sql = "select count(*) cnt from " + DBUtils.SCHEMA_NAME + "trade_rqmt_confirm_blob " +
                         "where trade_rqmt_confirm_id = @trade_rqmt_confirm_id";

            using (SqlConnection conn = new SqlConnection(sqlConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.Add("@trade_rqmt_confirm_id", System.Data.SqlDbType.Int).Value = pTradeRqmtConfirmId;
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
            return result;
        }

        public List<TradeRqmtConfirmBlobDto> GetAll()
        {
            const string selectTSqlCmd = @"SELECT ID, TRADE_RQMT_CONFIRM_ID, IMAGE_FILE_EXT, DOC_BLOB.PathName(), GET_FILESTREAM_TRANSACTION_CONTEXT()
                    FROM TRADE_RQMT_CONFIRM_BLOB";
            var result = new List<TradeRqmtConfirmBlobDto>();
            string serverPath;
            byte[] serverTxn;
            byte[] blobByteArray;
            using (TransactionScope ts = new TransactionScope())
            {
                using (SqlConnection conn = new SqlConnection(sqlConnStr))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(selectTSqlCmd, conn))
                    {
                        using (SqlDataReader dataReader = cmd.ExecuteReader())
                        {
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    serverPath = dataReader.GetSqlString(3).Value;
                                    serverTxn = dataReader.GetSqlBinary(4).Value;
                                    using (SqlFileStream sqlFileStr = new SqlFileStream(serverPath, serverTxn, FileAccess.Read))
                                    {
                                        using (MemoryStream memStream = new MemoryStream())
                                        {
                                            sqlFileStr.CopyTo(memStream);
                                            blobByteArray = memStream.ToArray();
                                        }
                                        sqlFileStr.Close();
                                    }
                                    result.Add(new TradeRqmtConfirmBlobDto
                                    {
                                        Id = DBUtils.HandleInt32IfNull(dataReader["ID"].ToString()),
                                        TradeRqmtConfirmId = DBUtils.HandleInt32IfNull(dataReader["TRADE_RQMT_CONFIRM_ID"].ToString()),
                                        ImageFileExt = dataReader["IMAGE_FILE_EXT"].ToString(),
                                        DocBlob = blobByteArray
                                    });
                                }
                            }
                        }
                    }
                }
                ts.Complete();
            }
            return result;
        }

        public TradeRqmtConfirmBlobDto Get(Int32 pTradeRqmtConfirmId)
        {
            const string selectTSqlCmd = @"SELECT ID, TRADE_RQMT_CONFIRM_ID, IMAGE_FILE_EXT, DOC_BLOB.PathName(), GET_FILESTREAM_TRANSACTION_CONTEXT()
                    FROM TRADE_RQMT_CONFIRM_BLOB WHERE TRADE_RQMT_CONFIRM_ID = @TRADE_RQMT_CONFIRM_ID";
            TradeRqmtConfirmBlobDto trqmtConfirmBlobResult = new TradeRqmtConfirmBlobDto();
            string serverPath;
            byte[] serverTxn;
            byte[] blobByteArray;
            using (TransactionScope ts = new TransactionScope())
            {
                using (SqlConnection conn = new SqlConnection(sqlConnStr))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(selectTSqlCmd, conn))
                    {
                        cmd.Parameters.Add("@TRADE_RQMT_CONFIRM_ID", SqlDbType.Int).Value = pTradeRqmtConfirmId;
                        using (SqlDataReader dataReader = cmd.ExecuteReader())
                        {

                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    serverPath = dataReader.GetSqlString(3).Value;
                                    serverTxn = dataReader.GetSqlBinary(4).Value;
                                    using (SqlFileStream sqlFileStr = new SqlFileStream(serverPath, serverTxn, FileAccess.Read))
                                    {
                                        using (MemoryStream memStream = new MemoryStream())
                                        {
                                            sqlFileStr.CopyTo(memStream);
                                            blobByteArray = memStream.ToArray();
                                        }
                                        sqlFileStr.Close();
                                    }

                                    trqmtConfirmBlobResult.Id = DBUtils.HandleInt32IfNull(dataReader["ID"].ToString());
                                    trqmtConfirmBlobResult.TradeRqmtConfirmId = DBUtils.HandleInt32IfNull(dataReader["TRADE_RQMT_CONFIRM_ID"].ToString());
                                    trqmtConfirmBlobResult.ImageFileExt = dataReader["IMAGE_FILE_EXT"].ToString();
                                    trqmtConfirmBlobResult.DocBlob = blobByteArray;
                                }
                            }
                        }
                    }
                }
                ts.Complete();
            }
            return trqmtConfirmBlobResult;
        }

        public Int32 Insert(TradeRqmtConfirmBlobDto pData)
        {
            const string insertTSql =
                @"INSERT INTO TRADE_RQMT_CONFIRM_BLOB(ID, TRADE_RQMT_CONFIRM_ID, IMAGE_FILE_EXT)
                VALUES(@ID, @TRADE_RQMT_CONFIRM_ID, @IMAGE_FILE_EXT);
                SELECT DOC_BLOB.PathName(), GET_FILESTREAM_TRANSACTION_CONTEXT()
                FROM TRADE_RQMT_CONFIRM_BLOB
                WHERE ID = @ID";

            int newId = DBUtils.GetNextSequence(sqlConnStr, SEQ_NAME);
            string serverPath;
            byte[] serverTxn;
            using (TransactionScope ts = new TransactionScope())
            {
                using (SqlConnection conn = new SqlConnection(sqlConnStr))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(insertTSql, conn))
                    {
                        cmd.Parameters.Add("@ID", SqlDbType.Int).Value = newId;
                        cmd.Parameters.Add("@TRADE_RQMT_CONFIRM_ID", SqlDbType.VarChar).Value = pData.TradeRqmtConfirmId;
                        cmd.Parameters.Add("@IMAGE_FILE_EXT", SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pData.ImageFileExt.ToUpper());
                        using (SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.SingleRow))
                        {
                            rdr.Read();
                            serverPath = rdr.GetSqlString(0).Value;
                            serverTxn = rdr.GetSqlBinary(1).Value;
                            rdr.Close();
                        }
                    }
                    //SaveDocImageFile(pFileName, serverPath, serverTxn);
                    SaveDocImageByteArray(pData.DocBlob, serverPath, serverTxn);
                }
                ts.Complete();
            }
            return newId;
        }

        public void Update(TradeRqmtConfirmBlobDto pData)
        {
            const string updateTSql =
                @"UPDATE TRADE_RQMT_CONFIRM_BLOB
                SET TRADE_RQMT_CONFIRM_ID = @TRADE_RQMT_CONFIRM_ID, IMAGE_FILE_EXT = @IMAGE_FILE_EXT
                WHERE TRADE_RQMT_CONFIRM_ID = @TRADE_RQMT_CONFIRM_ID
                SELECT TOP(1) DOC_BLOB.PathName(), GET_FILESTREAM_TRANSACTION_CONTEXT() 
                FROM TRADE_RQMT_CONFIRM_BLOB
                WHERE TRADE_RQMT_CONFIRM_ID = @TRADE_RQMT_CONFIRM_ID";

            using (TransactionScope ts = new TransactionScope())
            {
                using (SqlConnection conn = new SqlConnection(sqlConnStr))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(updateTSql, conn))
                    {
                        //cmd.Parameters.Add("@ID", SqlDbType.Int).Value = pData.Id;
                        cmd.Parameters.Add("@TRADE_RQMT_CONFIRM_ID", SqlDbType.VarChar).Value = pData.TradeRqmtConfirmId;
                        cmd.Parameters.Add("@IMAGE_FILE_EXT", SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pData.ImageFileExt.ToUpper());
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Get the pointer for file 
                                string path = reader.GetString(0);
                                byte[] transactionContext = reader.GetSqlBytes(1).Buffer;

                                // Create the SqlFileStream
                                using (Stream fileStream = new SqlFileStream(path, transactionContext, FileAccess.Write, FileOptions.SequentialScan, allocationSize: 0))
                                {
                                    // Write a byte array to the file. This will replace any data in the file.
                                    //fileStream.WriteByte(0x01);
                                    fileStream.Write(pData.DocBlob, 0, pData.DocBlob.Length);
                                }
                            }
                        }
                    }
                }
                ts.Complete();
            }
        }

        public Int32 Delete(Int32 pId)
        {
            Int32 rowsDeleted = 0;
            string sql = "delete from " + DBUtils.SCHEMA_NAME + "TRADE_RQMT_CONFIRM_BLOB where TRADE_RQMT_CONFIRM_ID = @TRADE_RQMT_CONFIRM_ID";

            using (SqlConnection conn = new SqlConnection(sqlConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.Add("@TRADE_RQMT_CONFIRM_ID", System.Data.SqlDbType.Int).Value = pId;
                    rowsDeleted = cmd.ExecuteNonQuery();
                }
            }
            return rowsDeleted;
        }

    }
}
