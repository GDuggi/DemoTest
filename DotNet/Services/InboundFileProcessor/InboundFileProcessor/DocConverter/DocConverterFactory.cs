using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace InboundFileProcessor
{
    public class DocConverterFactory
    {
        enum ConverterType   { LeadTool, Office,Tif,Unknown};
        public Logging _logFile;

        public void StartConverter()
        {
            //_logFile.WriteToLog("Instantiating LeadToolConverter.Startup()...");
            LeadToolConverter._logFile = _logFile;
            TifCopier._logFile = _logFile;

            LeadToolConverter.Startup();
            //_logFile.WriteToLog("LeadToolConverter.Startup() Complete.");
        }

        public void ShutdownConverter()
        {
            LeadToolConverter.Shutdown();
        }

        public IDocConverter GetDocConverter(string fileName)
        {
            try
            {
                _logFile.WriteToLog("Converter factory called for file: " + fileName);
                FileInfo fileInfo = new FileInfo(fileName);
                if (!fileInfo.Exists)
                {
                    _logFile.WriteToLog("Converter Factory - ERROR: " + fileName + " does not exist.");
                    return null;
                }
                string fileExtension = fileInfo.Extension.ToLower();
                ConverterType converter = getConvertType(fileExtension);
                if (converter == ConverterType.LeadTool)
                {
                    _logFile.WriteToLog("Lead tool object returned for " + fileName);
                    return LeadToolConverter.getInstance();
                }
                else if (converter == ConverterType.Tif)
                {
                    _logFile.WriteToLog("Tif Copier object returned for " + fileName);
                    return TifCopier.getInstance();
                }
            }
            catch (Exception e) {
                _logFile.WriteToLog("Converter Factory - ERROR: " + e.Message + "; Stack - " + e.StackTrace);
            }
            return null;
        }

        private ConverterType getConvertType(string fileExtension) {
            if (".tif".Equals(fileExtension, StringComparison.OrdinalIgnoreCase))
            {
                return ConverterType.Tif;
            }
            return ConverterType.LeadTool;
        }
    }
}
