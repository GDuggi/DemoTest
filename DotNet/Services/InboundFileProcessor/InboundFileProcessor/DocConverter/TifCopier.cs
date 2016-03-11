using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace InboundFileProcessor
{
    class TifCopier : IDocConverter       
    {
        private static TifCopier copier = new TifCopier();
        public static Logging _logFile;

        public void Convert(string origFile, string fromFile, string outputDir, string processedDir, string callerRef, string sentTo)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(fromFile);
                if (!fileInfo.Exists)
                {
                    _logFile.WriteToLog("TifCopier Converter: " + fromFile + " does not exist ");
                    return;
                }
                string srcFileName = fileInfo.Name;
                string outputFileName = outputDir + @"\" + srcFileName.Substring(0, srcFileName.IndexOf(".")) +"_"+ DateTime.Now.ToString("yyyyMMddhh24mmss")+ ".tif";
                _logFile.WriteToLog("TifCopier Converter - Started; src = " + srcFileName);

                FileUtils.CopyToDir(fileInfo, outputFileName);

                //jvc insert Inbound - Data Access here
                InboundController ibController = new InboundController();
                ibController._logFile = _logFile;
                ibController.ProcessFile(outputFileName, fromFile, callerRef, sentTo);
                if (Properties.Settings.Default.DeleteTIFEnabled)
                {
                    File.Delete(outputFileName);
                }

                _logFile.WriteToLog("TifCopier Converter - Completed; src = " + srcFileName);

            }
            catch (Exception e)
            {
                _logFile.WriteToLog("TifCopier - ERROR: " + e.Message + " Stack - " + e.StackTrace);
                throw e;
            }
            finally
            {
             
            }
        }
       public static TifCopier getInstance(){
           return copier;
       }
    }        
}
