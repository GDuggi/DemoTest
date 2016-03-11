using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using VaultUploader.Data.DbAccess;
using VaultUploader.Data.Models;
using VaultUploader.Data.Utils;

namespace VaultUploader.Data.Repository
{
   public class VaultedDocsRepository
    {
       private string _connectionString;

       private static string _selectSQL = "select vaulted_docs_id," +
                      "vaulted_docs_trade_rqmt_confirm_id," +
                      "vaulted_docs_associated_docs_id," +
                      "vaulted_docs_sent_flag," +
                      "vaulted_docs_metadata," +
                      "vaulted_docs_request_timestamp," +
                      "vaulted_docs_blob_id," +
                      "vaulted_docs_blob_image_file_ext," +
                      "rqmt_doc_type_rqmt_code," +
                      "rqmt_doc_type_sent_flag," +
                      "rqmt_doc_type_doc_type_code," +
                      "TRD_SYS_CODE," +
                      "TRD_SYS_TICKET " +
            "from " + DbContext.SCHEMA_NAME + "V_DOCS_TO_VAULT" + DbContext.NO_LOCK;

       private VaultedDocsRepository(string connectionString)
       {
           _connectionString = connectionString;
       }

       private static VaultedDocsRepository _vaultedDocsRepository;
       internal static VaultedDocsRepository Instance(string connectionString=null)
       {
           if (_vaultedDocsRepository == null)           
           {
               _vaultedDocsRepository = new VaultedDocsRepository(connectionString);
           }
           return _vaultedDocsRepository;
       }

       public List<VaultedDocs> GetAllUnposted()
       {
           return BindData(_selectSQL);
       }

       private List<VaultedDocs> BindData(string sql)
       {
           List<VaultedDocs> vaultDocs = new List<VaultedDocs>();
           using (SqlConnection conn = new SqlConnection(_connectionString))
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
                               vaultDocs.Add(new VaultedDocs
                               {
                                   VaultedDocsId = DbUtil.HandleInt32IfNull(dataReader["vaulted_docs_id"].ToString()),
                                   VaultedDocsTradeRqmtConfirmId = DbUtil.HandleInt32IfNull(dataReader["vaulted_docs_trade_rqmt_confirm_id"].ToString()),
                                   VaultedDocsAssociatedDocsId = DbUtil.HandleInt32IfNull(dataReader["vaulted_docs_associated_docs_id"].ToString()),
                                   VaultedDocsSentFlag = dataReader["vaulted_docs_sent_flag"].ToString(),
                                   VaultedDocsMetadata = dataReader["vaulted_docs_metadata"].ToString(),
                                   VaultedDocsRequestTimestamp = DbUtil.HandleDateTimeIfNull(dataReader["vaulted_docs_request_timestamp"].ToString()),
                                   VaultedDocsBlobId = DbUtil.HandleInt32IfNull(dataReader["vaulted_docs_blob_id"].ToString()),
                                   VaultedDocsBlobImageFileExt = dataReader["vaulted_docs_blob_image_file_ext"].ToString(),
                                   RqmtDocTypeRqmtCode = dataReader["rqmt_doc_type_rqmt_code"].ToString(),
                                   RqmtDocTypeSentFlag = dataReader["rqmt_doc_type_sent_flag"].ToString(),
                                   RqmtDocTypeDocTypeCode = dataReader["rqmt_doc_type_doc_type_code"].ToString(),
                                   TRDSYSCODE = dataReader["TRD_SYS_CODE"].ToString(),
                                   TRDSYSTICKET = dataReader["TRD_SYS_TICKET"].ToString()
                               });
                           }
                       }
                   }
               }
           }
           return vaultDocs;
       }

       public void CaptureVaultedDocsBlob(VaultedDocs vaultedDoc)
       {
          string sql = String.Format("SELECT ID,DOC_BLOB.PathName(),GET_FILESTREAM_TRANSACTION_CONTEXT() FROM {0}VAULTED_DOCS_BLOB{1} WHERE ID={2}"
              , DbContext.SCHEMA_NAME, DbContext.NO_LOCK,vaultedDoc.VaultedDocsBlobId);         
            using (TransactionScope ts = new TransactionScope())
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {                    
                        using (SqlDataReader dataReader = cmd.ExecuteReader())
                        {

                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    using (SqlFileStream sqlFileStr = new SqlFileStream(dataReader.GetSqlString(1).Value, dataReader.GetSqlBinary(2).Value, FileAccess.Read))
                                    {
                                        using (MemoryStream memStream = new MemoryStream())
                                        {
                                            sqlFileStr.CopyTo(memStream);
                                            vaultedDoc.VaultedDocsBlobDocBlob = memStream.ToArray();
                                        }
                                        sqlFileStr.Close();
                                    }                                    
                                }
                            }
                        }
                    }
                }
                ts.Complete();
            }                  
       }

       public int Update(VaultedDocs vaultedDocs, string URL)
       {
           Int32 rowsUpdated = 0;
           string sql = "UPDATE " + DbContext.SCHEMA_NAME + "VAULTED_DOCS " +
               " SET PROCESSED_FLAG = @PROCESSED_FLAG ,PROCESSED_TIMESTAMP= @PROCESSED_TIMESTAMP ,VAULT_GUID = @VAULT_GUID" +
               " WHERE ID = @ID";

           using (SqlConnection conn = new SqlConnection(_connectionString))
           {
               using (SqlCommand cmd = new SqlCommand(sql, conn))
               {
                   cmd.CommandType = System.Data.CommandType.Text;
                   cmd.Parameters.Add("@ID", System.Data.SqlDbType.Int).Value = vaultedDocs.VaultedDocsId;
                   cmd.Parameters.Add("@PROCESSED_FLAG", System.Data.SqlDbType.VarChar).Value = "Y";
                   cmd.Parameters.Add("@PROCESSED_TIMESTAMP", System.Data.SqlDbType.DateTime2).Value = DateTime.Now;//the service runs on GMT ..
                   cmd.Parameters.Add("@VAULT_GUID", System.Data.SqlDbType.VarChar).Value = URL;

                   conn.Open();
                   rowsUpdated = cmd.ExecuteNonQuery();
               }
           }
           return rowsUpdated;
       }
    }
}
