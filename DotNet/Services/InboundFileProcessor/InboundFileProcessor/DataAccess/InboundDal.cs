using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;

namespace InboundFileProcessor.DataAccess
{
    public class InboundDal : IInboundDal
    {
        public Logging _logFile;
        private const string SCHEMA_NAME = "ConfirmMgr.";
        private const string SEQ_NAME_DOCS = "seq_inbound_docs";
        private const string SEQ_NAME_BLOB = "seq_inbound_docs_blob";
        private SqlConnection sqlConnIntegrated = null;
        private SqlTransaction sqlTransIntegrated = null;
        private string sqlConnStrIntegrated = null;

        public InboundDal(string pSqlConnStrIntegrated)
        {
            sqlConnStrIntegrated = pSqlConnStrIntegrated;
        }

        public Int32 Insert(string pMarkupFileName, string pOrigFileName, InboundDocsDto pDataDocs, InboundDocsBlobDto pDataBlob)
        {
            Int32 newIdDocs = 0;
            Int32 newIdBlob = 0;

            try
            {
                sqlConnIntegrated = new SqlConnection(sqlConnStrIntegrated);
                sqlConnIntegrated.Open();
                sqlTransIntegrated = sqlConnIntegrated.BeginTransaction("inboundTransIntegrated");

                using (sqlConnIntegrated)
                {
                    string sqlSEQ = "SELECT NEXT VALUE FOR " + SCHEMA_NAME + SEQ_NAME_DOCS;
                    using (SqlCommand cmdSEQDoc = new SqlCommand(sqlSEQ, sqlConnIntegrated, sqlTransIntegrated))
                    {
                        cmdSEQDoc.CommandType = System.Data.CommandType.Text;
                        var seqNo = cmdSEQDoc.ExecuteScalar();
                        newIdDocs = Convert.ToInt32(seqNo);
                    }

                    string sql = "Insert into " + SCHEMA_NAME + "inbound_docs " +
                        "   (id, caller_ref, sent_to, rcvd_ts, file_name, sender, cmt, doc_status_code, has_auto_ascted_flag, " +
                               " mapped_cpty_sn, mapped_brkr_sn, mapped_cdty_code, job_ref ) " +
                        " Values " +
                        "   (@id, @caller_ref, @sent_to, @rcvd_ts, @file_name, @sender, @cmt, @doc_status_code, @has_auto_ascted_flag, " +
                                " @mapped_cpty_sn, @mapped_brkr_sn, @mapped_cdty_code, @job_ref ) ";

                    using (SqlCommand cmd = new SqlCommand(sql, sqlConnIntegrated, sqlTransIntegrated))
                    {
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = newIdDocs;
                        cmd.Parameters.Add("@caller_ref", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pDataDocs.CallerRef);
                        cmd.Parameters.Add("@sent_to", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pDataDocs.SentTo);
                        cmd.Parameters.Add("@rcvd_ts", System.Data.SqlDbType.DateTime).Value = DBUtils.ValueDateTimeOrDbNull(pDataDocs.RcvdTs);
                        cmd.Parameters.Add("@file_name", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pDataDocs.FileName);
                        cmd.Parameters.Add("@sender", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pDataDocs.Sender);
                        cmd.Parameters.Add("@cmt", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pDataDocs.Cmt);
                        cmd.Parameters.Add("@doc_status_code", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pDataDocs.DocStatusCode);
                        cmd.Parameters.Add("@has_auto_ascted_flag", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pDataDocs.HasAutoAsctedFlag);
                        cmd.Parameters.Add("@mapped_cpty_sn", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pDataDocs.MappedCptySn);
                        cmd.Parameters.Add("@mapped_brkr_sn", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pDataDocs.MappedBrkrSn);
                        cmd.Parameters.Add("@mapped_cdty_code", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pDataDocs.MappedCdtyCode);
                        cmd.Parameters.Add("@job_ref", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pDataDocs.JobRef);
                        cmd.ExecuteNonQuery();
                    }

                    sqlSEQ = "SELECT NEXT VALUE FOR " + SCHEMA_NAME + SEQ_NAME_BLOB;
                    using (SqlCommand cmdSEQBlob = new SqlCommand(sqlSEQ, sqlConnIntegrated, sqlTransIntegrated))
                    {
                        cmdSEQBlob.CommandType = System.Data.CommandType.Text;
                        var seqNo = cmdSEQBlob.ExecuteScalar();
                        newIdBlob = Convert.ToInt32(seqNo);
                    }

                    string insertTSql =
                    @"INSERT INTO " + SCHEMA_NAME + @"INBOUND_DOCS_BLOB(ID, INBOUND_DOCS_ID, ORIG_IMAGE_FILE_EXT, MARKUP_IMAGE_FILE_EXT)
                      VALUES(@ID, @INBOUND_DOCS_ID, @ORIG_IMAGE_FILE_EXT, @MARKUP_IMAGE_FILE_EXT);
                      SELECT ORIG_IMAGE_BLOB.PathName(),MARKUP_IMAGE_BLOB.PathName(), GET_FILESTREAM_TRANSACTION_CONTEXT()
                      FROM " + SCHEMA_NAME + @"INBOUND_DOCS_BLOB WHERE ID = @ID";

                    string origServerPath;
                    string markupServerPath;
                    byte[] serverTxn;

                    using (SqlCommand cmd = new SqlCommand(insertTSql, sqlConnIntegrated, sqlTransIntegrated))
                    {
                        cmd.Parameters.Add("@ID", SqlDbType.Int).Value = newIdBlob;
                        cmd.Parameters.Add("@INBOUND_DOCS_ID", SqlDbType.VarChar).Value = newIdDocs;
                        cmd.Parameters.Add("@ORIG_IMAGE_FILE_EXT", SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pDataBlob.OrigImageFileExt.ToUpper().Replace(".", ""));
                        cmd.Parameters.Add("@MARKUP_IMAGE_FILE_EXT", SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pDataBlob.MarkupImageFileExt.ToUpper().Replace(".", ""));
                        using (SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.SingleRow))
                        {
                            rdr.Read();
                            origServerPath = rdr.GetSqlString(0).Value;
                            markupServerPath = rdr.GetSqlString(1).Value;
                            serverTxn = rdr.GetSqlBinary(2).Value;
                            rdr.Close();
                        }
                    }
                    SaveDocImageFile(pOrigFileName, origServerPath, serverTxn);
                    SaveDocImageFile(pMarkupFileName, markupServerPath, serverTxn);
                    sqlTransIntegrated.Commit();
                }
            }
            catch (Exception e)
            {
                _logFile.WriteToLog("Inbound Controller - ERROR: " + e.Message + "; Stack - " + e.StackTrace);
                try
                {
                    sqlTransIntegrated.Rollback();
                }
                catch (Exception ex1)
                {
                    _logFile.WriteToLog("Inbound Controller - Rollback (Integrated) ERROR - Type: " + ex1.GetType() + "; Message: " + ex1.Message);
                }
                throw e;
            }
            finally
            {
                if (sqlConnIntegrated != null && sqlConnIntegrated.State != ConnectionState.Closed)
                {
                    sqlConnIntegrated.Close();
                }
            }
            return newIdBlob;
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
    }
}
