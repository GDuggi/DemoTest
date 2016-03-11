using InboundFileProcessor.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace InboundFileProcessor
{
    public class DirectoryProcessor
    {
        private const string ROOTDIR = "ROOTDIR";
        private const string THUMBS_DB = "Thumbs.db";
        private string[] _scanDirs;
        private List<string> _validExtList;
        private string _outputDir;
        private string _processedDir;
        private string _CurrentRootDir;
        private Logging _debugLog;
        public Logging _EventLog;
        public bool _isDebugLogEnabled = false;

        private string _failedDir;

        public DirectoryProcessor(string[] pScanDirs, string pValidFileExt, string pOutputDir, string pProcessedDir, string pFailedDir, Logging pLogger)
        {
            if (pLogger == null)
            {
                _debugLog = new Logging();
            }
            else
            {
                _debugLog = pLogger;
            }
            _scanDirs = pScanDirs;
            _validExtList = pValidFileExt.Split(',').ToList<string>();
            _outputDir = initDirectory(pOutputDir);
            _processedDir = initDirectory(pProcessedDir);

            //jvc
            _failedDir = initDirectory(pFailedDir);
            initDirectory(pFailedDir + @"\" + Properties.Settings.Default.FailedSubDirUnsupported);
            initDirectory(pFailedDir + @"\" + Properties.Settings.Default.FailedSubDirConvert);
        }

        private String initDirectory(String pDir)
        {
            string result = pDir;
            if (!Directory.Exists(pDir))
            {
                DirectoryInfo directoryInfo = Directory.CreateDirectory(pDir);
                result = directoryInfo.FullName;
            }
            return result;
        }

        private void writeToDebugLog(string pMethod, string pMessage)
        {
            if (_isDebugLogEnabled)
                _debugLog.WriteToLog(pMethod + ": " + pMessage, "Debug_DirectoryProcessor");
        }
        
        public void ProcessDirectories()
        {
            _CurrentRootDir = "";
            foreach (string path in _scanDirs)
            {
                _CurrentRootDir = path;
                writeToDebugLog("ProcessDirectories", "_CurrentRootDir=" + _CurrentRootDir);
                if (File.Exists(path))
                {
                    ProcessFile(path);
                }
                else if (Directory.Exists(path))
                {
                    ProcessDirectory(path);
                }
                else
                {
                    writeToDebugLog("ProcessDirectories", "_CurrentRootDir(" + path + ") is not a valid file or directory.");
                    _EventLog.WriteToLog("_CurrentRootDir(" + path + ") is not a valid file or directory.");
                }
            }  
        }

        // Process all files in the directory passed in, recurse on any directories  
        // that are found, and process the files they contain. 
        public void ProcessDirectory(string targetDirectory)
        {
            writeToDebugLog("ProcessDirectory", "Processing: " + targetDirectory );
            // Process the list of files found in the directory. 
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
                ProcessFile(fileName);

            // Recurse into subdirectories of this directory. 
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                ProcessDirectory(subdirectory);

            writeToDebugLog("ProcessDirectory", "Processing Complete: " + targetDirectory);
        }

        public void ProcessFile(string fileNameAndPath)
        {
            try
            {
                writeToDebugLog("ProcessFile", "----------------------------------------------------------");
                writeToDebugLog("ProcessFile", "Processing: " + fileNameAndPath );
                string extWithPeriod = Path.GetExtension(fileNameAndPath);
                string ext = extWithPeriod.Replace(".", "");

                string callerRef = getCallerRef(fileNameAndPath);
                //writeToDebugLog("ProcessFile", "callerRef=" + callerRef);

                string sentTo = getSentTo(fileNameAndPath);
                //writeToDebugLog("ProcessFile", "sentTo=" + sentTo);

                if (fileNameAndPath.Contains(THUMBS_DB))
                    return;
                else if (_validExtList.Contains(ext,StringComparer.OrdinalIgnoreCase))
                {
                    writeToDebugLog("ProcessFile", "ext=" + ext + " is a valid extension. Processing...");

                    writeToDebugLog("ProcessFile", "DocConverter - Initializing.");
                    Controller controller = new Controller();
                    controller._logFile = _debugLog;
                    writeToDebugLog("ProcessFile", "DocConverter - Sending sourceFile=" + fileNameAndPath + "; failedDir=" + this._failedDir + "; processedDir=" + this._processedDir);
                    controller.ProcessFile(fileNameAndPath, this._outputDir, this._failedDir, "", callerRef, sentTo);
                    writeToDebugLog("ProcessFile", "DocConverter - Completed.");

                    //Move original data file to processed dir
                    string origNameOnly = Path.GetFileName(fileNameAndPath);
                    string datafileProcessedNameAndPath = Path.Combine(_processedDir, origNameOnly);
                    bool processedFileExists = File.Exists(datafileProcessedNameAndPath);
                    writeToDebugLog("ProcessFile", "Processed sourceFile(" + datafileProcessedNameAndPath + ") exists=" + processedFileExists);
                    if (File.Exists(fileNameAndPath)) //jvc make sure not already moved due to fail
                    {
                        writeToDebugLog("ProcessFile", "Moving sourcefile to processedDir: " + datafileProcessedNameAndPath);
                        FileUtils.MoveToDir(new FileInfo(fileNameAndPath), _processedDir);
                        writeToDebugLog("ProcessFile", "Move file complete.");
                    }

                    writeToDebugLog("ProcessFile", "Processing Complete: " + fileNameAndPath);
                    _EventLog.WriteToLog("--> ProcessFile - Complete: " + fileNameAndPath);
                }
                else
                {
                    writeToDebugLog("ProcessFile", "Skipped File: " + fileNameAndPath);
                    _EventLog.WriteToLog("-->** Skipped File: " + fileNameAndPath);
                    string mailMsg = "The file ''"+fileNameAndPath+"'' was skipped because it is not in a supported format ("+ Properties.Settings.Default.ValidFileExt+")."+Environment.NewLine+
                                        "It has been moved to the directory " + _failedDir + @"\" + Properties.Settings.Default.FailedSubDirUnsupported + ".";
                    MailUtils.SendEmail("Warning", "Unsupported File Format", mailMsg, fileNameAndPath);
                    FileUtils.MoveToDir(new FileInfo(fileNameAndPath), _failedDir  + @"\" + Properties.Settings.Default.FailedSubDirUnsupported);

                }

                //Israel 5/13/2015 -- address generated name collision.
                int milliseconds = 2000;
                Thread.Sleep(milliseconds);
            }
            catch (FormatConversionException ex) {
                writeToDebugLog("ProcessFile", "ERROR: " + ex.Message);
                _EventLog.WriteToLog("ERROR Processing: " + fileNameAndPath + "; " + ex.Message);
            }
            catch (Exception e)
            {
                string erMsg = "ERROR Processing: " + fileNameAndPath + "; " + Environment.NewLine + e.Message + "." + Environment.NewLine + Environment.NewLine + 
                               "Stack Trace:" + Environment.NewLine + e.StackTrace;
                writeToDebugLog("ProcessFile", erMsg);
                _EventLog.WriteToLog(erMsg);
                if (File.Exists(fileNameAndPath))
                {
                    string mailMsg = "The file ''" + fileNameAndPath + "'' has been moved to the directory " + this._failedDir + ";" + Environment.NewLine + Environment.NewLine + 
                                     "Reason:" + Environment.NewLine + "There was an issue while processing this file.  " + e.Message + "." + Environment.NewLine + Environment.NewLine + 
                                     "Stack Trace:" + Environment.NewLine + e.StackTrace;
                    MailUtils.SendEmail("Error", "Exception Processing Inbound File", mailMsg, fileNameAndPath);
                    writeToDebugLog("ProcessFile", "Moving sourcefile to failedDir: " + fileNameAndPath);
                    FileUtils.MoveToDir(new FileInfo(fileNameAndPath), this._failedDir);
                    writeToDebugLog("ProcessFile", "Move file complete.");
                }
                else
                {
                    string mailMsg = "There was an exception processing file ''" + fileNameAndPath + "'';" + Environment.NewLine + Environment.NewLine + "Reason:" + Environment.NewLine + e.Message + "." + Environment.NewLine + Environment.NewLine + "Stack Trace:" + Environment.NewLine + e.StackTrace;
                    MailUtils.SendEmail("Error", "Exception Processing Inbound File", mailMsg, "");
                }
            }
        }

        private string GenerateFileName()
        {
            string scannedFileName = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
            return scannedFileName;
        }

        private string getCallerRef(string pFileName)
        {
            //Strip everything except immediate folder parent from pFileName
            string result = String.Empty;
            string nameOnly = Path.GetFileName(pFileName);
            result = @pFileName.Replace(_CurrentRootDir + @"\", "").Replace(nameOnly, "");
            if (result.EndsWith(@"\")) {
                result = result.Substring(0, result.Length - 1);
            }
            return @result;
        }

        private string getSentTo(string pFileName)
        {
            string result = String.Empty;
            string currentRoot = Path.GetDirectoryName(pFileName);
            DirectoryInfo di = System.IO.Directory.GetParent(pFileName);
            if (currentRoot == _CurrentRootDir)
                result = ROOTDIR;
            else
                result = di.Name;

            return result;
        }


    }
}
