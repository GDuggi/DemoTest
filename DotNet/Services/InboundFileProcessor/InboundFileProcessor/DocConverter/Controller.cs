using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using InboundFileProcessor.Common;

namespace InboundFileProcessor
{
    public class Controller
    {
        public Logging _logFile;

        public Controller()
		{
		}

        public void ProcessFile(string fileNameAndPath, string outputDir, string failedDir, string processedDir,string callerRef, string sentTo)
        {
            
            DocConverterFactory factory = new DocConverterFactory();
            factory._logFile = _logFile;
            factory.StartConverter();
            FileInfo file = new FileInfo(fileNameAndPath);
            try
                {
                   IDocConverter converter = factory.GetDocConverter(file.FullName);
                   converter.Convert(file.FullName, file.FullName, outputDir, processedDir, callerRef, sentTo);
                }
                catch (Exception e)
                {
                    string mailMsg = "The file ''" + file.FullName + "'' has been moved to the directory " + failedDir + @"\" + Properties.Settings.Default.FailedSubDirConvert + ";" + Environment.NewLine + Environment.NewLine +
                                     "Reason:" + Environment.NewLine + "There was an issue converting this file to TIF format.  " + Environment.NewLine + e.Message;
                    MailUtils.SendEmail("Error", "Converting File",mailMsg, fileNameAndPath);
                    FileUtils.MoveToDir(file, failedDir + @"\" + Properties.Settings.Default.FailedSubDirConvert);
                    throw new FormatConversionException("Error: ''" + file.FullName + "'' has been moved to the directory " + failedDir + "; " + Environment.NewLine + e.Message);
                }
            
            factory.ShutdownConverter();
        }
    }
}
