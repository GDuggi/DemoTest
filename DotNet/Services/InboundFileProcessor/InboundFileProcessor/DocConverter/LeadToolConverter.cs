using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Leadtools;
using Leadtools.Codecs;

namespace InboundFileProcessor
{
    class LeadToolConverter : IDocConverter
    {
        private static LeadToolConverter converter = new LeadToolConverter();
        private static RasterCodecs codec = null;
        //private static System.Configuration.AppSettingsReader reader = null;
        private static string _scanDir = String.Empty;
        public static Logging _logFile;

        private LeadToolConverter()
        {
        }

        public static void Startup()
        {
            //_logFile.WriteToLog("Applying Leadtools license...");
            string licenseFileName = "";
            string licenseFileWithLocation = "";
            string developerKey = "";

            try
            { 
                string currDir = System.Reflection.Assembly.GetEntryAssembly().Location;
                currDir = Path.GetDirectoryName(currDir);
                licenseFileName = Properties.Settings.Default.LeadtoolsLicenseFile;
                licenseFileWithLocation = currDir + @"\" + licenseFileName;
                developerKey = Properties.Settings.Default.LeadtoolsDeveloperKey;

                //_logFile.WriteToLog("Applying license at: " + licenseFileWithLocation);
                //_logFile.WriteToLog("Applying key: " + developerKey);

                RasterSupport.SetLicense(licenseFileWithLocation, developerKey);
                //_logFile.WriteToLog("Applied Leadtools license.");

                codec = new RasterCodecs();
                //_logFile.WriteToLog("Lead tools Codec object is created");

            }
            catch (Exception e)
            {
                _logFile.WriteToLog("Problem applying Leadtools license:");
                _logFile.WriteToLog("LeadtoolsLicenseFile = " + licenseFileName);
                _logFile.WriteToLog("licenseFileWithLocation = " + licenseFileWithLocation);
                _logFile.WriteToLog("LeadtoolsLicenseFile = " + developerKey);
                _logFile.WriteToLog(e.StackTrace + "\nInner Exception: " + e.InnerException);
                throw e;
            }
        }

        public static void Shutdown()
        {
            if (codec != null)
            {
                codec.Dispose();
            }
        }

        public static LeadToolConverter getInstance()
        {
            return converter;
        }

        public void Convert(string origFile, string fromFile, string outputDir, string processedDir, string callerRef, string sentTo)
        {
            RasterImage image = null;
            try
            {
                FileInfo fileInfo = new FileInfo(fromFile);
                if (!fileInfo.Exists)
                {
                    _logFile.WriteToLog("Lead Tool Converter - ERROR: " + fromFile + " does not exist.");
                    return;
                }

                string srcFileName = fileInfo.Name;
                _logFile.WriteToLog("Lead Tool Converter: Conversion starting.");
                string srcFileNameWithoutExt = Path.GetFileNameWithoutExtension(srcFileName);
                string outputFileName = outputDir + @"\" + srcFileNameWithoutExt + "_" + DateTime.Now.ToString("yyyyMMddhh24mmss") + ".tif";


                string fileExt = fileInfo.Extension.ToLower();
                LoadOptionValues(codec, fileExt);
                CodecsImageInfo info = codec.GetInformation(fromFile, true);

                image = codec.Load(fromFile, 0, CodecsLoadByteOrder.Bgr, 1, info.TotalPages);

                codec.Save(image, outputFileName, RasterImageFormat.Tif, 1, 1, info.TotalPages, 1, CodecsSavePageMode.Overwrite);

                _logFile.WriteToLog("Lead Tool Converter: Conversion completed.");
                //jvc insert Inbound - Data Access here
                InboundController ibController = new InboundController();
                ibController._logFile = _logFile;
                _logFile.WriteToLog("InboundController Started: Saving Info to Database for source file:" + origFile + " and converted file: " + outputFileName);
                ibController.ProcessFile(outputFileName, origFile, callerRef, sentTo); //jvc tif and original source format
                if (Properties.Settings.Default.DeleteTIFEnabled)
                {
                    File.Delete(outputFileName);
                }
                _logFile.WriteToLog("InboundController Completed: File Info saved to Database.");

            }
            catch (RasterException ex)
            {
                string erMsg = "";
                if (ex.Code == RasterExceptionCode.FileFormat)
                    erMsg = "The file ''" + fromFile + "'' does not contain an image format recognizable by LEADTOOLS;" + Environment.NewLine + ex.Message;
                else if (ex.Code == RasterExceptionCode.InvalidFormat)
                    erMsg = "The file ''" + fromFile + "'' does not contain an format recognizable by LEADTOOLS;" + Environment.NewLine + ex.Message;
                else
                    erMsg = "Could not load the file ''" + fromFile + "''; " + Environment.NewLine + "Leadtools code:  " + ex.Code + "; message:  " + ex.Message;
                _logFile.WriteToLog(erMsg);
                throw new Exception(erMsg);
            }
            catch (Exception e)
            {
                _logFile.WriteToLog("Lead Tool Converter - ERROR: " + e.Message + "; Stack - " + e.StackTrace);
                throw e;
            }
            finally
            {
                if (image != null)
                {
                    image.Dispose();
                }
            }
        }

        private void LoadOptionValues(RasterCodecs codec, string fileExtension )
        {

            if (".pdf".Equals(fileExtension,StringComparison.OrdinalIgnoreCase))
            {
                codec.Options.Pdf.InitialPath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);//_scanDir;
                
                //_logFile.WriteToLog("PDF initial path = " + codec.Options.Pdf.InitialPath);
                codec.Options.RasterizeDocument.Load.XResolution = 204;
                codec.Options.RasterizeDocument.Load.YResolution = 196;
                //_logFile.WriteToLog("PDF options are loaded.");
            }
            else if (".rtf".Equals(fileExtension, StringComparison.OrdinalIgnoreCase))
            {
                codec.Options.RasterizeDocument.Load.TopMargin = 0.5;
                codec.Options.RasterizeDocument.Load.BottomMargin = 0.5;
                codec.Options.RasterizeDocument.Load.LeftMargin = 0.5;
                codec.Options.RasterizeDocument.Load.RightMargin = 0.5;
                codec.Options.RasterizeDocument.Load.PageHeight = 14;
                codec.Options.RasterizeDocument.Load.PageWidth = 8.5;
                //_logFile.WriteToLog("RTF options are loaded");
            }
            else if (".txt".Equals(fileExtension,StringComparison.OrdinalIgnoreCase))
            {

                codec.Options.Txt.Load.Enabled = true;
                codec.Options.RasterizeDocument.Load.TopMargin = 1.5;
                codec.Options.RasterizeDocument.Load.BottomMargin = 1.5;
                codec.Options.RasterizeDocument.Load.LeftMargin = 1;
                codec.Options.RasterizeDocument.Load.RightMargin = 1;
                codec.Options.Txt.Load.FontColor = new RasterColor(255, 0, 0);
                codec.Options.Txt.Load.FontSize = 12;
                //_logFile.WriteToLog("TXT options are loaded");
            }
        }
   
    }
}
