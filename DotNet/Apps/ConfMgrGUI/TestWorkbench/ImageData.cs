using DevExpress.XtraPdfViewer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;

namespace TestWorkbench
{
    public class ImageData
    {
        //private const string ConnStr = "Data Source=.;Integrated Security=True;Initial Catalog=TestImage;";
        //private const string ConnStr = @"Server=CNF01INFDBS01\SQLSVR11;UID=ifrankel;Password=Oracle11;Database=TestImage;";
        //private const string ConnStr = @"Server=CNF01INFDBS01\SQLSVR11;Integrated Security=True;Database=TestImage;";
        private const string ConnStr = @"database=TestImage;server=CNF01INFDBS01\SQLSVR11;integrated security=sspi;";
        public static string TEMP_FILENAME =  @"C:\Users\ifrankel\AppDev\Test\Temp\TestDoc1.";

        #region "Insert Image"

        //Save
        public static void InsertDocImage(int imageId, string desc, string filename, string imagetype)
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
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(InsertTSql, conn))
                    {
                        cmd.Parameters.Add("@ImageId", SqlDbType.Int).Value = imageId;
                        cmd.Parameters.Add("@Description", SqlDbType.VarChar).Value = desc;
                        cmd.Parameters.Add("@ImageType", SqlDbType.VarChar).Value = imagetype.ToUpper();
                        using (SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.SingleRow))
                        {
                            rdr.Read();
                            serverPath = rdr.GetSqlString(0).Value;
                            serverTxn = rdr.GetSqlBinary(1).Value;
                            rdr.Close();
                        }
                    }
                    SaveDocImageFile(filename, serverPath, serverTxn);
                }
                ts.Complete();
            }
        }

        private static void SaveDocImageFile(string clientPath, string serverPath, byte[] serverTxn)
        {
            const int BlockSize = 1024 * 512;
            using (FileStream source = new FileStream(clientPath, FileMode.Open, FileAccess.Read))
            {
                using (SqlFileStream dest = new SqlFileStream(serverPath, serverTxn, FileAccess.Write))
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

        #endregion

        #region "Select Image"


        //Retrieve
        public static void SelectDocImage(int ImageId, out string desc, out string fileext)
        {
            const string SelectTSql = @"
                SELECT
                Description, ImageType, DocImage.PathName(), GET_FILESTREAM_TRANSACTION_CONTEXT()
                FROM InboundImages
                WHERE ImageId = @ImageId";
            //Image docImage;
            string serverPath;
            byte[] serverTxn;
            using (TransactionScope ts = new TransactionScope())
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(SelectTSql, conn))
                    {
                        cmd.Parameters.Add("@ImageId", SqlDbType.Int).Value = ImageId;
                        using (SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.SingleRow))
                        {
                            rdr.Read();
                            desc = rdr.GetSqlString(0).Value;
                            fileext = rdr.GetSqlString(1).Value;
                            serverPath = rdr.GetSqlString(2).Value;
                            serverTxn = rdr.GetSqlBinary(3).Value;
                            rdr.Close();
                        }
                    }
                    //docImage = LoadDocImage(serverPath, serverTxn, fileext);
                    LoadDocImageIntoFile(serverPath, serverTxn, fileext);
                }
                ts.Complete();
            }
            //return desc;
        }

        private static void LoadDocImageIntoFile(string filePath, byte[] txnToken, string imageType)
        {
            using (SqlFileStream sfs = new SqlFileStream(filePath, txnToken, FileAccess.Read))
            {
                string fileName = TEMP_FILENAME + imageType;
                using (FileStream fstream = new FileStream(fileName, FileMode.Create))
                {
                    sfs.CopyTo(fstream, 4096);
                }
                sfs.Close();
            }            
        }

        private static Image LoadDocImage(string filePath, byte[] txnToken, string imageType)
        {
            Image docImage = null;
            Bitmap bmap;

            using (SqlFileStream sfs = new SqlFileStream(filePath, txnToken, FileAccess.Read))
            {
                if (imageType.Equals("TIF"))
                    docImage = Image.FromStream(sfs);
                else //if (imageType.Equals("PDF"))
                {
                    //MemoryStream memStream = new MemoryStream();
                    //storeStream.SetLength(sfs.Length);
                    //sfs.CopyTo(memStream);
                    //docImage = Image.FromStream(memStream, false, false);

                    //System.Drawing.ImageConverter converter = new System.Drawing.ImageConverter();
                    //byte[] m_Bytes = ReadToEnd(memStream);
                    //docImage = (Image)converter.ConvertFrom(m_Bytes);

                    FileStream fstream = new FileStream(TEMP_FILENAME, FileMode.Create);
                    sfs.CopyTo(fstream, 4096);

                    //docImage = Image.FromFile(TEMP_FILENAME);
                    docImage = Image.FromStream(fstream);

                    //docImage = (Image)bmap;
                    
                }
                sfs.Close();
            } 
            return docImage;
        }

        #endregion

        public static byte[] ReadToEnd(System.IO.Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }

    }
}
