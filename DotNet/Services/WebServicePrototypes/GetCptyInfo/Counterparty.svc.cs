using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using log4net;

namespace Counterparty
{
    public class Counterparty : ICounterparty
    {

        public GetAgreementListResponse getAgreementList(GetAgreementListRequest getAgreementListRequest)
        {
            try
            {
                Log.Info("Counterparty GetAgreementListRequest: " + (getAgreementListRequest == null ? "null" : getAgreementListRequest.ToString()));

                //var client = new GetCptyInfoService.CptyInfoClient();
                //var response = client.getAgreementList(request);

                var agreements = new Agreement[] { new Agreement("AgreementType1") };
                var response = new GetAgreementListResponse(agreements);

                Log.Info("Counterparty GetAgreementListResponse: " + (response == null ? "null" : response.ToString()));
                return response;
            }
            catch (Exception e)
            {
                Log.Error("Failed to process getAgreementList() service call", e);
                throw e;
            }

        }

        public GetDocumentSendToResponse getDocumentSendTo(GetDocumentSendToRequest getDocumentSendToRequest)
        {
            try
            {
                Log.Info("Counterparty GetDocumentSendToRequest: " + (getDocumentSendToRequest == null ? "null" : getDocumentSendToRequest.ToString()));
                var sendTos = new SendTo[] { new SendTo("rnell@amphorainc.com") };

                var response = new GetDocumentSendToResponse(sendTos);
                Log.Info("Counterparty GetDocumentSendToResponse: " + (response == null ? "null" : response.ToString()));
                return response;
            }
            catch (Exception e)
            {
                Log.Error("Failed to process getDocumentSendTo() service call", e);
                throw e;
            }
        }

        public StoreDocumentSendToResponse storeDocumentSendTo(StoreDocumentSendToRequest storeDocumentSendToRequest)
        {
            try
            {
                Log.Info("Counterparty StoreDocumentSendToRequest: " + (storeDocumentSendToRequest == null ? "null" : storeDocumentSendToRequest.ToString()));

                var response = new StoreDocumentSendToResponse(true);
                Log.Info("Counterparty StoreDocumentSendToResponse: " + (response == null ? "null" : response.ToString()));
                return response;
            }
            catch (Exception e)
            {
                Log.Error("Failed to process storeDocumentSendTo() service call", e);
                throw e;
            }
        }

        private static ILog Log
        {
            get { return LogManager.GetLogger(typeof(Counterparty)); }
        }

    }
}
