using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Transactions;

namespace DBAccess.SqlServer
{
    public class ImagesDal : IImagesDal
    {
        private readonly string sqlConnStr = "";

        public ImagesDal(string pSqlConn)
        {
            sqlConnStr = pSqlConn;
        }

        public ImagesDto GetByDocId(Int32 docsId, ImagesDtoType imagesDtoType)
        {
            ImagesDto imagesDto = null;
            using (var ts = new TransactionScope())
            {
                using (var conn = new SqlConnection(sqlConnStr))
                {
                    conn.Open();
                    var selectTSqlCmd = GetSelectByDocIdSql(imagesDtoType);
                    using (var cmd = new SqlCommand(selectTSqlCmd, conn))
                    {
                        cmd.Parameters.AddWithValue("@DocsId", docsId);
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            if (dataReader.Read())
                            {
                                var serverTxn = dataReader.GetSqlBinary(4).Value;
                                imagesDto = new ImagesDto(
                                    DBUtils.HandleInt32IfNull(dataReader["DOCS_ID"].ToString()),
                                    ReadBytesFromSqlFileStream(Convert.ToString(dataReader["MARKUP_IMAGE_PATH_NAME"]), serverTxn),
                                    Convert.ToString(dataReader["MARKUP_IMAGE_FILE_EXT"]),
                                    ReadBytesFromSqlFileStream(Convert.ToString(dataReader["ORIG_IMAGE_PATH_NAME"]), serverTxn),
                                    Convert.ToString(dataReader["ORIG_IMAGE_FILE_EXT"]),
                                    imagesDtoType,
                                    Convert.ToInt32(dataReader["ID"])
                                    );                                
                            }
                        }
                    }
                }
                ts.Complete();
            }
            return imagesDto;
        }

        public Int32 Insert(ImagesDto pData)
        {
            var insertTSql = GenerateInsertSql(pData);
            var newId = GetNextRowId(pData);
            using (var ts = new TransactionScope())
            {
                using (var conn = new SqlConnection(sqlConnStr))
                {
                    var irr = PerformRowInsert(pData, conn, insertTSql, newId);
                    SaveDocImage(pData.MarkupImage, irr.MarkupServerPath, irr.ServerTransaction);
                    SaveDocImage(pData.OriginalImage, irr.OriginalServerPath, irr.ServerTransaction);
                }
                ts.Complete();
            }
            return newId;
        }

        public Int32 Insert(string originalFileName, string markupFileName, ImagesDto pData)
        {
            var insertTSql = GenerateInsertSql(pData);
            var newId = GetNextRowId(pData);
            using (var ts = new TransactionScope())
            {
                using (var conn = new SqlConnection(sqlConnStr))
                {
                    var irr = PerformRowInsert(pData, conn, insertTSql, newId);
                    SaveDocImageFile(markupFileName, irr.MarkupServerPath, irr.ServerTransaction);
                    SaveDocImageFile(originalFileName, irr.OriginalServerPath, irr.ServerTransaction);
                }
                ts.Complete();
            }
            return newId;
        }

        public void Update(ImagesDto pData)
        {
            var updateSql = String.Format(
                @"UPDATE {0}{1}
	                    set {2} = @DocsId,
	                    ORIG_IMAGE_FILE_EXT = @OrigImageFileExt,
	                    MARKUP_IMAGE_FILE_EXT = @MarkupImageFileExt
	                    where ID = @Id;
                  SELECT 
	                    ORIG_IMAGE_BLOB.PathName(),
	                    MARKUP_IMAGE_BLOB.PathName(), 
	                    GET_FILESTREAM_TRANSACTION_CONTEXT()
	                    FROM {0}{1} WHERE ID = @Id",
                DBUtils.SCHEMA_NAME,
                (pData.Type == ImagesDtoType.Inbound) ? "INBOUND_DOCS_BLOB" : "ASSOCIATED_DOCS_BLOB",
                (pData.Type == ImagesDtoType.Inbound) ? "INBOUND_DOCS_ID" : "ASSOCIATED_DOCS_ID");

            using (var ts = new TransactionScope())
            {
                using (var conn = new SqlConnection(sqlConnStr))
                {
                    conn.Open();
                    ImagesServerInformation imagesServerInfo;
                    using (var cmd = new SqlCommand(updateSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", pData.ImageId);
                        cmd.Parameters.AddWithValue("@DocsId", pData.DocsId);
                        cmd.Parameters.AddWithValue("@OrigImageFileExt",
                            DBUtils.ValueStringOrDBNull(pData.OriginalImageFileExt.ToUpper().Replace(".", "")));
                        cmd.Parameters.AddWithValue("@MarkupImageFileExt",
                            DBUtils.ValueStringOrDBNull(pData.MarkupImageFileExt.ToUpper().Replace(".", "")));
                        imagesServerInfo = ReadImagesServerInformation(cmd);
                    }

                    SaveDocImage(pData.MarkupImage, imagesServerInfo.MarkupServerPath,
                        imagesServerInfo.ServerTransaction);
                    SaveDocImage(pData.OriginalImage, imagesServerInfo.OriginalServerPath,
                        imagesServerInfo.ServerTransaction);
                }
                ts.Complete();
            }
        }

        public void Delete(ImagesDto pData)
        {
            string deleteSql = String.Format(@"DELETE from {0}{1} where ID = @ImageId",
                DBUtils.SCHEMA_NAME,
                (pData.Type == ImagesDtoType.Inbound) ? "INBOUND_DOCS_BLOB" : "ASSOCIATED_DOCS_BLOB");

            using (var conn = new SqlConnection(sqlConnStr))
            {
                conn.Open();
                using (var cmd = new SqlCommand(deleteSql, conn))
                {
                    cmd.Parameters.AddWithValue("@ImageId", pData.ImageId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public ImagesDto SwitchImagesDtoType(ImagesDto pData, Int32 newDocId)
        {
            using (var ts = new TransactionScope())
            {
                ImagesDtoType newType = pData.Type == ImagesDtoType.Inbound
                    ? ImagesDtoType.Associated
                    : ImagesDtoType.Inbound;

                ImagesDto newDto = new ImagesDto(newDocId, pData.MarkupImage, pData.MarkupImageFileExt, pData.OriginalImage, pData.OriginalImageFileExt, newType);
                Insert(newDto);

                Delete(pData);

                ts.Complete();
                return newDto;
            }                       
        }

        private void SaveDocImageFile(string pFileNameAndPath, string pServerPath, byte[] pServerTxn)
        {
            using (var source = new FileStream(pFileNameAndPath, FileMode.Open, FileAccess.Read))
            {
                WriteSqlFileStream(pServerPath, pServerTxn, source);
            }
        }

        private void SaveDocImage(byte[] contents, string serverPath, byte[] serverTxn)
        {
            using (var source = new MemoryStream(contents, false))
            {
                WriteSqlFileStream(serverPath, serverTxn, source);
            }
        }

        private static void WriteSqlFileStream(string pServerPath, byte[] pServerTxn, Stream source)
        {
            using (var dest = new SqlFileStream(pServerPath, pServerTxn, FileAccess.Write))
            {
                const int BlockSize = 1024*512;
                var buffer = new byte[BlockSize];
                int bytesRead;
                while ((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
                {
                    dest.Write(buffer, 0, bytesRead);
                    dest.Flush();
                }
                dest.Close();
            }
        }

        private string GetSelectByDocIdSql(ImagesDtoType imagesDtoType)
        {
            var docsIdFieldName = (imagesDtoType == ImagesDtoType.Inbound) ? "INBOUND_DOCS_ID" : "ASSOCIATED_DOCS_ID";

            return string.Format(@"SELECT
	                                    ID,	
	                                    {0} as DOCS_ID,
	                                    ORIG_IMAGE_FILE_EXT,
	                                    ORIG_IMAGE_BLOB.PathName() as ORIG_IMAGE_PATH_NAME,
	                                    GET_FILESTREAM_TRANSACTION_CONTEXT(),
	                                    MARKUP_IMAGE_FILE_EXT,
	                                    MARKUP_IMAGE_BLOB.PathName() as MARKUP_IMAGE_PATH_NAME
                                   FROM {1}{2} WHERE {3} = @DocsId ",
                docsIdFieldName,
                DBUtils.SCHEMA_NAME,
                (imagesDtoType == ImagesDtoType.Inbound) ? "INBOUND_DOCS_BLOB" : "ASSOCIATED_DOCS_BLOB",
                docsIdFieldName);
        }

        private static byte[] ReadBytesFromSqlFileStream(string serverPath, byte[] serverTxn)
        {
            byte[] blobByteArray;
            using (var sqlFileStr = new SqlFileStream(serverPath, serverTxn, FileAccess.Read))
            {
                using (var memStream = new MemoryStream())
                {
                    sqlFileStr.CopyTo(memStream);
                    blobByteArray = memStream.ToArray();
                }
                sqlFileStr.Close();
            }
            return blobByteArray;
        }

        private static string GenerateInsertSql(ImagesDto pData)
        {
            var imagesDtoType = pData.Type;
            var insertTSql =
                String.Format(
                    @"INSERT INTO {0}{1}(ID, {2}, ORIG_IMAGE_FILE_EXT, MARKUP_IMAGE_FILE_EXT)
                VALUES(@ID, @DOCS_ID, @ORIG_IMAGE_FILE_EXT, @MARKUP_IMAGE_FILE_EXT);
                SELECT ORIG_IMAGE_BLOB.PathName(),MARKUP_IMAGE_BLOB.PathName(), GET_FILESTREAM_TRANSACTION_CONTEXT()
                FROM {0}{1} INBOUND_DOCS_BLOB WHERE ID = @ID",
                    DBUtils.SCHEMA_NAME,
                    (imagesDtoType == ImagesDtoType.Inbound) ? "INBOUND_DOCS_BLOB" : "ASSOCIATED_DOCS_BLOB",
                    (imagesDtoType == ImagesDtoType.Inbound) ? "INBOUND_DOCS_ID" : "ASSOCIATED_DOCS_ID");
            return insertTSql;
        }

        private int GetNextRowId(ImagesDto pData)
        {
            var seqName = (pData.Type == ImagesDtoType.Inbound)
                ? "SEQ_INBOUND_DOCS_BLOB"
                : "SEQ_ASSOCIATED_DOCS_BLOB";

            var newId = DBUtils.GetNextSequence(sqlConnStr, seqName);
            return newId;
        }

        private static ImagesServerInformation PerformRowInsert(ImagesDto pData, SqlConnection conn, string insertTSql,
            int newId)
        {
            conn.Open();

            using (var cmd = new SqlCommand(insertTSql, conn))
            {
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = newId;
                cmd.Parameters.Add("@DOCS_ID", SqlDbType.VarChar).Value = pData.DocsId;
                cmd.Parameters.Add("@ORIG_IMAGE_FILE_EXT", SqlDbType.VarChar).Value =
                    DBUtils.ValueStringOrDBNull(pData.OriginalImageFileExt.ToUpper().Replace(".", ""));
                cmd.Parameters.Add("@MARKUP_IMAGE_FILE_EXT", SqlDbType.VarChar).Value =
                    DBUtils.ValueStringOrDBNull(pData.MarkupImageFileExt.ToUpper().Replace(".", ""));
                return ReadImagesServerInformation(cmd);
            }
        }

        private static ImagesServerInformation ReadImagesServerInformation(SqlCommand cmd)
        {
            using (var rdr = cmd.ExecuteReader(CommandBehavior.SingleRow))
            {
                rdr.Read();
                var irr = new ImagesServerInformation
                {
                    OriginalServerPath = rdr.GetSqlString(0).Value,
                    MarkupServerPath = rdr.GetSqlString(1).Value,
                    ServerTransaction = rdr.GetSqlBinary(2).Value
                };
                return irr;
            }
        }

        private class ImagesServerInformation
        {
            public string OriginalServerPath { get; set; }
            public string MarkupServerPath { get; set; }
            public byte[] ServerTransaction { get; set; }
        }
    }
}