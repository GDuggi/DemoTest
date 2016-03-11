
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using log4net;
using System.IO;
using System.Reflection;

namespace GetDocument
{
    public class GetDocument : IGetDocument
    {

        public GetDealSheetResponse getDealSheet(GetDealSheetRequest getDealSheetRequest)
        {
            try
            {
                Log.Info("GetDocument GetDealSheetRequest: " + (getDealSheetRequest == null ? "null" : getDealSheetRequest.ToString()));

                byte[] b = getFile("SampleDealsheet.html");

                var response = new GetDealSheetResponse(ObjectFormatInd.HTML, b );

                Log.Info("GetDocument GetDealSheetResponse: " + (response == null ? "null" : response.ToString()));
                return response;
            }
            catch (Exception e)
            {
                Log.Error("Failed to process getDealSheet() service call", e);
                throw e;
            }
        }

        public GetConfirmationResponse getConfirmation(GetConfirmationRequest getConfirmationRequest)
        {
            try
            {
                Log.Info("GetDocument GetConfirmationRequest: " + (getConfirmationRequest == null ? "null" : getConfirmationRequest.ToString()));

                byte[] b = getFile("SampleConfirm.docx");

                var response = new GetConfirmationResponse(ObjectFormatInd.DOCX, true, b);

                Log.Info("GetDocument GetConfirmationResponse: " + (response == null ? "null" : response.ToString()));
                return response;
            }
            catch (Exception e)
            {
                Log.Error("Failed to process getConfirmation() service call", e);
                throw e;
            }
        }

        byte[] getFile(String fileName)
        {
            String path = Path.Combine(AssemblyDirectory, fileName);
            Log.InfoFormat("Loading file {0}", path);
            byte[] b = File.ReadAllBytes(path);
            return b;
        }

        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        private static ILog Log
        {
            get { return LogManager.GetLogger(typeof(GetDocument)); }
        }
    }
}
