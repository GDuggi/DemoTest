using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;

namespace CommonUtils
{
    public class SempraFileUtils
    {
        private const string PROJ_FILE_NAME = "SempraFileUtils";
        public static FileInfo[] GetFilesInDirSortedByDate(string directory, string FilePattern, bool bAscOrder)
        {
            FileInfo[] fiArr;
            try
            {
                // make a reference to our directory
                DirectoryInfo di = new DirectoryInfo(directory);
                // get a reference to each file (matching our pattern if necessary)
                if ((FilePattern != null) & (FilePattern.Length != 0) & (FilePattern
                != "*.*"))
                    fiArr = di.GetFiles(FilePattern);
                else
                    fiArr = di.GetFiles();

                // sort it as needed
                if (bAscOrder)
                    Array.Sort(fiArr, new SortFileInfoByDateAsc());
                else
                    Array.Sort(fiArr, new SortFileInfoByDateDesc());

                return fiArr;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving FileInfo for the following values:" + Environment.NewLine +
                    "Directory: " + directory + ", File Pattern: " + FilePattern + ", Asending Order?" + bAscOrder + Environment.NewLine +
                     "Error CNF-335 in " + PROJ_FILE_NAME + ".GetFilesInDirSortedByDate()" + ex.Message);
            }
        }

        public class SortFileInfoByDateAsc : IComparer
        {
            int IComparer.Compare(object x, object y)
            {
                // Perform the comparison.
                return DateTime.Compare(((FileInfo)x).CreationTime,
                ((FileInfo)y).CreationTime);
            }
        }

        public class SortFileInfoByDateDesc : IComparer
        {
            int IComparer.Compare(object x, object y)
            {
                // Perform the comparison.
                return (-1) * DateTime.Compare(((FileInfo)x).CreationTime,
                ((FileInfo)y).CreationTime);
            }
        }
    }
}
