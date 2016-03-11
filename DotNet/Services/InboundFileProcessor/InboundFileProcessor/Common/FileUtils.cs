using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace InboundFileProcessor
{
    public static class FileUtils
    {

        public static void CopyToDir(FileInfo fi, string toDir) 
        {
            fi.CopyTo(toDir, true);
        }

        public static void MoveToDir(FileInfo fi, string toDir)
        {
            string srcFileName = fi.Name;
            string destFileName = toDir + @"\" + fi.Name;
            MoveFile(fi.FullName, destFileName);
        }

        private static void MoveFile(string srcFileName, string destFileName)
        {
            FileInfo fileInfo = new FileInfo(srcFileName);
            if (File.Exists(destFileName))
            {
                File.Delete(destFileName);
            }
            if (fileInfo.Exists)
            {
                fileInfo.MoveTo(destFileName);
            }
        }
    }
}
