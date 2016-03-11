using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;

namespace CommonUtils
{
    public class CompareSMLFXExposureFileInfo : IComparer
    {
        public int Compare(object x, object y)
        {
            string fileDate = "";
            int year;
            int month;
            int day;

            DateTime date1;
            DateTime date2;


            FileInfo file1 = new FileInfo((string)x);
            FileInfo file2 = new FileInfo((string)y);

            string justFileName = file1.Name;

            fileDate = justFileName.Remove(0, 10);
            // format for this date is yy-mm-dd, i.e. 070322
            year = 2000 + Convert.ToInt16(fileDate.Substring(0, 2));
            month = Convert.ToInt16(fileDate.Substring(2, 2));
            day = Convert.ToInt16(fileDate.Substring(4, 2));
            date1 = new DateTime(year, month, day);

            justFileName = file2.Name;

            fileDate = justFileName.Remove(0, 10);
            // format for this date is yy-mm-dd, i.e. 070322
            year = 2000 + Convert.ToInt16(fileDate.Substring(0, 2));
            month = Convert.ToInt16(fileDate.Substring(2, 2));
            day = Convert.ToInt16(fileDate.Substring(4, 2));
            date2 = new DateTime(year, month, day);


            return DateTime.Compare(date1, date2);
        }
    }

    public class CompareLogFileInfo : IComparer
    {
        public int Compare(object x, object y)
        {
            int iResult;
            FileInfo fia = new FileInfo((string)x);
            FileInfo fib = new FileInfo((string)y);

            if (fia.LastWriteTime == fib.LastWriteTime)
            {
                iResult = 0;
            }
            else if (fia.LastWriteTime > fib.LastWriteTime)
            {
                iResult = 1;
            }
            else iResult = -1;

            return iResult;
        }
    }

    public class CompareSMLFlashFileInfo : IComparer
    {
        public int Compare(object x, object y)
        {
            string fileDate = "";
            int year;
            int month;
            int day;

            DateTime date1;
            DateTime date2;


            FileInfo file1 = new FileInfo((string)x);
            FileInfo file2 = new FileInfo((string)y);

            string justFileName = file1.Name;  // FLASH YYYY MM DD

            fileDate = justFileName.Remove(0, 6);  //YYYY MM DD

            year = Convert.ToInt16(fileDate.Substring(0, 4));
            month = Convert.ToInt16(fileDate.Substring(5, 2));
            day = Convert.ToInt16(fileDate.Substring(8, 2));
            date1 = new DateTime(year, month, day);

            justFileName = file2.Name;

            fileDate = justFileName.Remove(0, 6);  //YYYY MM DD

            year = Convert.ToInt16(fileDate.Substring(0, 4));
            month = Convert.ToInt16(fileDate.Substring(5, 2));
            day = Convert.ToInt16(fileDate.Substring(8, 2));
            date2 = new DateTime(year, month, day);


            return DateTime.Compare(date1, date2);
        }
    }

    public class CompareSMLSmartFeedFileInfo : IComparer
    {
        public int Compare(object x, object y)
        {
            string fileDate = "";
            int year;
            int month;
            int day;

            DateTime date1;
            DateTime date2;


            FileInfo file1 = new FileInfo((string)x);
            FileInfo file2 = new FileInfo((string)y);

            string justFileName = file1.Name;  // YYYY MM DD

            fileDate = justFileName.Remove(0, 6);  //YYYY MM DD

            year = Convert.ToInt16(fileDate.Substring(0, 4));
            month = Convert.ToInt16(fileDate.Substring(5, 2));
            day = Convert.ToInt16(fileDate.Substring(8, 2));
            date1 = new DateTime(year, month, day);

            justFileName = file2.Name;

            fileDate = justFileName.Remove(0, 6);  //YYYY MM DD

            year = Convert.ToInt16(fileDate.Substring(0, 4));
            month = Convert.ToInt16(fileDate.Substring(5, 2));
            day = Convert.ToInt16(fileDate.Substring(8, 2));
            date2 = new DateTime(year, month, day);


            return DateTime.Compare(date1, date2);
        }
    }
}
